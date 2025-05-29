## 关于嵌入unity组件
## Unity生成的资产
https://drive.google.com/file/d/1XuqEIxqqCAdbcWXLAyk7TosMHvSrhhbW/view?usp=drive_link
## 构建
`Unity Editor` -> `File` -> `Build Settings` -> `Universal Windows Platform` -> `SwitchPlatform`  
按下图进行构建  
![image](https://github.com/MicaGames/InkBall/assets/68675068/4014a3a8-afec-41d2-b434-5709d854b9f7)  

随便你构建到哪个文件夹

打开构建结果，运行里面的 sln文件，生成一遍。若提示找不到mobile引用，则在引用中删除该引用

你需要把`Il2CppOutputProject`,`UnityCommon.props`,`Players` 三个放到`AppWeather`同级目录

`Data`文件夹放入`AppWeather`文件夹内,并且之后每次更新Unity都这样替换


![yjs9bh.png](https://files.catbox.moe/yjs9bh.png)

随后打开`AppWeather.sln`

![pel668.png](https://files.catbox.moe/pel668.png)

对着项目右键-> `生成依赖项`->`项目依赖项`

需要确定生成顺序如下

![94tndz.png](https://files.catbox.moe/94tndz.png)

就能直接跑了

