/*
 * Hello Minecraft! Launcher
 * Copyright (C) 2020  huangyuhui <huanghongxun2008@126.com> and contributors
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */
package org.jackhuang.hmcl.download;

import org.jackhuang.hmcl.game.Artifact;
import org.jackhuang.hmcl.game.CompatibilityRule;
import org.jackhuang.hmcl.game.GameRepository;
import org.jackhuang.hmcl.game.Library;
import org.jackhuang.hmcl.game.Version;
import org.jackhuang.hmcl.game.VersionLibraryBuilder;
import org.jackhuang.hmcl.task.Task;
import org.jackhuang.hmcl.util.Logging;
import org.jackhuang.hmcl.util.SimpleMultimap;
import org.jackhuang.hmcl.util.gson.JsonUtils;
import org.jackhuang.hmcl.util.versioning.VersionNumber;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.StandardCopyOption;
import java.util.*;
import java.util.logging.Level;
import java.util.stream.Collectors;

import static org.jackhuang.hmcl.download.LibraryAnalyzer.LibraryType.FORGE;
import static org.jackhuang.hmcl.download.LibraryAnalyzer.LibraryType.LITELOADER;
import static org.jackhuang.hmcl.download.LibraryAnalyzer.LibraryType.OPTIFINE;

public class MaintainTask extends Task<Version> {
    private final GameRepository repository;
    private final Version version;

    public MaintainTask(GameRepository repository, Version version) {
        this.repository = repository;
        this.version = version;

        if (version.getInheritsFrom() != null)
            throw new IllegalArgumentException("MaintainTask requires independent game version");
    }

    @Override
    public void execute() {
        setResult(maintain(repository, version));
    }

    public static Version maintain(GameRepository repository, Version version) {
        if (version.getInheritsFrom() != null)
            throw new IllegalArgumentException("MaintainTask requires independent game version");

        String mainClass = version.resolve(null).getMainClass();

        if (mainClass != null && mainClass.equals(LibraryAnalyzer.LAUNCH_WRAPPER_MAIN)) {
            return maintainOptiFineLibrary(repository, maintainGameWithLaunchWrapper(unique(version), true), false);
        } else if (mainClass != null && mainClass.equals(LibraryAnalyzer.MOD_LAUNCHER_MAIN)) {
            // Forge 1.13 and OptiFine
            return maintainOptiFineLibrary(repository, maintainGameWithModLauncher(repository, unique(version)), true);
        } else {
            // Vanilla Minecraft does not need maintain
            // Fabric does not need maintain, nothing compatible with fabric now.
            return maintainOptiFineLibrary(repository, unique(version), false);
        }
    }

    public static Version maintainPreservingPatches(GameRepository repository, Version version) {
        if (!version.isResolvedPreservingPatches())
            throw new IllegalArgumentException("MaintainTask requires independent game version");
        Version newVersion = maintain(repository, version.resolve(repository));
        return newVersion.setPatches(version.getPatches()).markAsUnresolved();
    }

    private static Version maintainGameWithLaunchWrapper(Version version, boolean reorderTweakClass) {
        LibraryAnalyzer libraryAnalyzer = LibraryAnalyzer.analyze(version);
        VersionLibraryBuilder builder = new VersionLibraryBuilder(version);
        String mainClass = null;

        if (!libraryAnalyzer.has(FORGE)) {
            builder.removeTweakClass("forge");
        }

        // Installing Forge will override the Minecraft arguments in json, so LiteLoader and OptiFine Tweaker are being re-added.

        if (libraryAnalyzer.has(LITELOADER) && !libraryAnalyzer.hasModLauncher()) {
            builder.replaceTweakClass("liteloader", "com.mumfrey.liteloader.launch.LiteLoaderTweaker", !reorderTweakClass);
        } else {
            builder.removeTweakClass("liteloader");
        }

        if (libraryAnalyzer.has(OPTIFINE)) {
            if (!libraryAnalyzer.has(LITELOADER) && !libraryAnalyzer.has(FORGE)) {
                builder.replaceTweakClass("optifine", "optifine.OptiFineTweaker", !reorderTweakClass);
            } else {
                if (libraryAnalyzer.hasModLauncher()) {
                    // If ModLauncher installed, we use ModLauncher in place of LaunchWrapper.
                    mainClass = "cpw.mods.modlauncher.Launcher";
                    builder.replaceTweakClass("optifine", "optifine.OptiFineForgeTweaker", !reorderTweakClass);
                } else {
                    // If forge or LiteLoader installed, OptiFine Forge Tweaker is needed.
                    builder.replaceTweakClass("optifine", "optifine.OptiFineForgeTweaker", !reorderTweakClass);
                }

            }
        } else {
            builder.removeTweakClass("optifine");
        }

        Version ret = builder.build();
        return mainClass == null ? ret : ret.setMainClass(mainClass);
    }

    private static Version maintainGameWithModLauncher(GameRepository repository, Version version) {
        LibraryAnalyzer libraryAnalyzer = LibraryAnalyzer.analyze(version);
        VersionLibraryBuilder builder = new VersionLibraryBuilder(version);

        if (!libraryAnalyzer.has(FORGE)) return version;

        if (libraryAnalyzer.has(OPTIFINE)) {
            Library hmclTransformerDiscoveryService = new Library(new Artifact("org.jackhuang.hmcl", "transformer-discovery-service", "1.0"));
            Optional<Library> optiFine = version.getLibraries().stream().filter(library -> library.is("optifine", "OptiFine")).findAny();
            boolean libraryExisting = version.getLibraries().stream().anyMatch(library -> library.is("org.jackhuang.hmcl", "transformer-discovery-service"));
            optiFine.ifPresent(library -> {
                builder.addJvmArgument("-Dhmcl.transformer.candidates=${libraries_directory}/" + library.getPath());
                if (!libraryExisting) builder.addLibrary(hmclTransformerDiscoveryService);
                Path libraryPath = repository.getLibraryFile(version, hmclTransformerDiscoveryService).toPath();
                try {
                    Files.createDirectories(libraryPath.getParent());
                    Files.copy(MaintainTask.class.getResourceAsStream("/assets/game/HMCLTransformerDiscoveryService-1.0.jar"),
                            libraryPath, StandardCopyOption.REPLACE_EXISTING);
                } catch (IOException e) {
                    Logging.LOG.log(Level.WARNING, "Unable to unpack HMCLTransformerDiscoveryService", e);
                }
            });
        }

        return builder.build();
    }

    private static Version maintainOptiFineLibrary(GameRepository repository, Version version, boolean remove) {
        LibraryAnalyzer libraryAnalyzer = LibraryAnalyzer.analyze(version);
        List<Library> libraries = new ArrayList<>(version.getLibraries());

        if (libraryAnalyzer.has(OPTIFINE)) {
            if (libraryAnalyzer.has(LITELOADER) || libraryAnalyzer.has(FORGE)) {
                // If forge or LiteLoader installed, OptiFine Forge Tweaker is needed.
                // And we should load the installer jar instead of patch jar.
                if (repository != null) {
                    for (int i = 0; i < version.getLibraries().size(); ++i) {
                        Library library = libraries.get(i);
                        if (library.is("optifine", "OptiFine")) {
                            Library newLibrary = new Library(new Artifact("optifine", "OptiFine", library.getVersion(), "installer"));
                            if (repository.getLibraryFile(version, newLibrary).exists()) {
                                libraries.set(i, null);
                                // OptiFine should be loaded after Forge in classpath.
                                // Although we have altered priority of OptiFine higher than Forge,
                                // there still exists a situation that Forge is installed without patch.
                                // Here we manually alter the position of OptiFine library in classpath.
                                if (!remove) libraries.add(newLibrary);
                            }
                        }

                        if (library.is("optifine", "launchwrapper-of")) {
                            // With MinecraftForge installed, the custom launchwrapper installed by OptiFine will conflicts
                            // with the one installed by MinecraftForge or LiteLoader or ModLoader.
                            // Simply removing it works.
                            libraries.set(i, null);
                        }
                    }
                }
            }
        }

        return version.setLibraries(libraries.stream().filter(Objects::nonNull).collect(Collectors.toList()));
    }

    public static boolean isPurePatched(Version version) {
        if (!version.isResolvedPreservingPatches())
            throw new IllegalArgumentException("isPurePatched requires a version resolved preserving patches");

        return version.hasPatch("game");
    }

    public static Version unique(Version version) {
        List<Library> libraries = new ArrayList<>();

        SimpleMultimap<String, Integer> multimap = new SimpleMultimap<String, Integer>(HashMap::new, LinkedList::new);

        for (Library library : version.getLibraries()) {
            String id = library.getGroupId() + ":" + library.getArtifactId();
            VersionNumber number = VersionNumber.asVersion(library.getVersion());
            String serialized = JsonUtils.GSON.toJson(library);

            if (multimap.containsKey(id)) {
                boolean duplicate = false;
                for (int otherLibraryIndex : multimap.get(id)) {
                    Library otherLibrary = libraries.get(otherLibraryIndex);
                    VersionNumber otherNumber = VersionNumber.asVersion(otherLibrary.getVersion());
                    if (CompatibilityRule.equals(library.getRules(), otherLibrary.getRules())) { // rules equal, ignore older version.
                        boolean flag = true;
                        if (number.compareTo(otherNumber) > 0) { // if this library is newer
                            // replace [otherLibrary] with [library]
                            libraries.set(otherLibraryIndex, library);
                        } else if (number.compareTo(otherNumber) == 0) { // same library id.
                            // prevent from duplicated libraries
                            if (library.equals(otherLibrary)) {
                                String otherSerialized = JsonUtils.GSON.toJson(otherLibrary);
                                // A trick, the library that has more information is better, which can be
                                // considered whose serialized JSON text will be longer.
                                if (serialized.length() > otherSerialized.length()) {
                                    libraries.set(otherLibraryIndex, library);
                                }
                            } else {
                                // for text2speech, which have same library id as well as version number,
                                // but its library and native library does not equal
                                flag = false;
                            }
                        }
                        if (flag) {
                            duplicate = true;
                            break;
                        }
                    }
                }

                if (!duplicate) {
                    multimap.put(id, libraries.size());
                    libraries.add(library);
                }
            } else {
                multimap.put(id, libraries.size());
                libraries.add(library);
            }
        }

        return version.setLibraries(libraries);
    }
}