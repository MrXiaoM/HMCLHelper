# HMCLHelper
硬核解决HMCL微软登录白屏问题

## 原理
修改HMCL的代码，让HMCL无法使用它自身的浏览器进行微软登录，而要传参进去  
然后写一个自动处理网页操作的浏览器，让用户登录并获取需要的参数  
再让用户把参数复制粘贴传进HMCL里，让HMCL来获取 Token 来进行登录  
~~因为懒~~因为技术力低，目前传参的输入框显示的还是“用户名”，但不影响操作  
实测可登录、获取皮肤、进入Hypixel  

## 微软登录直链

[click here](https://login.live.com/oauth20_authorize.srf?client_id=00000000402b5328&response_type=code&scope=service%3A%3Auser.auth.xboxlive.com%3A%3AMBI_SSL&redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf)  
如果你的浏览器已经登录过微软账号了，会直接转跳到空白页  
空白页的网址中的参数 code 就是**原理**中需要的参数

## 修改HMCL

仓库地址: [https://github.com/huanghongxun/HMCL](https://github.com/huanghongxun/HMCL)

>你需要对 HMCL 3.3.181 (commit 13fa713d58d1699cdbb560dc430e6efc3d83138c) 作出如下修改
>
>**移除 org.jackhuang.hmcl.ui.account.MicrosoftAccountLoginStage**(可不移除，没有必要)  
>
>**修改 org.jackhuang.hmcl.ui.Controllers**  
>> 移除初始化 MicrosoftAccountLoginStage 的初始化部分 (如不执行上面那一条，这里也没有必要)
>
>**修改 org.jackhuang.hmcl.ui.account.AddAccountPane**   
>> 将 validateUsername(String username) 中的 if (loginType == Accounts.FACTORY_OFFLINE)   
>> 改成 if (loginType == Accounts.FACTORY_OFFLINE || loginType == Accounts.FACTORY_MICROSOFT)  
>  
>**修改 org.jackhuang.hmcl.auth.microsoft.MicrosoftAccount**  
>> 添加字段 String code = "N/A";  
>> 将构造函数中的 CharacterSelector characterSelector 改成 String code  
>>    
>> 把该构造函数中的 MicrosoftSession acquiredSession = service.authenticate();  
>> 改成  
>> this.code = code;  
>> MicrosoftSession acquiredSession = service.authenticate(code);  
>>  
>> 在 logIn()，把 MicrosoftSession acquiredSession = service.authenticate();  
>> 改成  
>> if(code != "N/A"){  
>> MicrosoftSession acquiredSession = service.authenticate(code);  
>> 然后authenticated = true;后面那里再补个}  
>  
>**修改 org.jackhuang.hmcl.auth.microsoft.MicrosoftAccountFactory**  
>> getLoginType() 的返回值改成 AccountLoginType.USERNAME  
>> create(CharacterSelector, String, String, Object) 的返回值改成 new MicrosoftAccount(service, username)  
>  
>**修改 org.jackhuang.hmcl.auth.microsoft.MicrosoftService**  
>> 注释掉以下内容:  
>> - 字段 private final WebViewCallback callback;  
>> - 构建函数中的 this.callback = callback;  
>> - authenticate() 方法中的 requireNonNull(callback);  
>> - authenticate() 方法中的 String code=之后到.get();的一长串  
>> - authenticate() 方法中捕捉的 InterruptedException 和 ExecutionException 异常部分

修改完毕的源代码请见分支`hmcl`，修改好并编译好的HMCL在Release  

**我赌上我全网的名声发誓：我绝对不会往里面添加任何后门代码**  
本仓库的登录方法是我通过分析HMCL代码研究出来的，仅供参考，希望对你有所帮助
