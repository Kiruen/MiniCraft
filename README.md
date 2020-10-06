# MiniCraft

大四的毕设，用C#+OpenGL4.1从零实现了一个类似Minecraft的沙盒游戏。
由于时间仓促+从头学起GL，仅仅实现了最基本的部分，比如摄像机、天空盒、物体的拾取(用来实现交互、销毁)、物理引擎、游戏输入控制系统等，可以绘制不同材质的方块（泥土、玻璃、树叶、水、门(具有交互性)），地图使用噪声算法随机生成（很逼近MC了，还可以生成平原、岩石层、胡泊、海洋、树林、花丛、村庄等基础地形模块），而且实现了地图区块的动态装卸（比较流畅了）；地图还可以保存、加载（使用和MC一致的地图格式），还有一个简陋的命令系统（比如tp、模型的复制粘贴）。
✔自认为写的很烂很烂很烂，1.2万行代码摆在那跟一堆戳满洞的shit一样。唯一振奋人心的就是通过一晚上的努力使帧数达到40FPS以上，内存占用维持在200MB左右(之前甚至到了1GB...)，可以比较流畅(=凑合能看)地实现每帧10w+方块的动态绘制。
其他的部分，非常非常非常无聊，用答辩老师的话来说：“你这个充其量就是算法的实现，不能叫游戏吧。。” hhhh...
不打算再维护了，因为我算看明白了，能不依赖引擎从头撸游戏的是真大佬。我也没那个耐心了，以后有兴趣做游戏，我会考虑使用成熟的引擎，可以少走很多弯路。
