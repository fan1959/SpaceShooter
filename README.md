# 🚀 SpaceShooter - 经典太空射击游戏的个人实现与技术总结

[![Platform](https://img.shields.io/badge/Platform-Windows-blue.svg)](https://github.com/fan1959/SpaceShooter/releases)
[![Engine](https://img.shields.io/badge/Engine-Unity_2022.3.62f2c1_LTS-brightgreen.svg)](https://unity.com/)

本仓库是我的第一个 Unity 游戏开发实战项目，基于 Unity 官方经典案例 **《SpaceShooter》（太空纵版射击）** 进行个人独立还原与拓展。

> 🎮 **[点击右侧 Releases 栏目，下载编译好的游戏成品 (解压即玩)！](https://github.com/fan1959/SpaceShooter/releases)** 

---

## 🎮 游戏简介 (About the Game)

这是一款经典的复古科幻风格太空纵版射击游戏。玩家需要驾驶太空战机，在危机四伏的 XZ 物理平面内（忽略 Y 轴高度）移动，通过精妙的操作躲避撞击并消灭敌人，挑战自己的生存极限！

### 🕹️ 操作说明 (How to Play)
*   **移动 (Move)：** 键盘 `W` `A` `S` `D` 或 **方向键** 控制战机在屏幕范围内移动。
*   **射击 (Shoot)：** 点击 **`鼠标左键`** 发射激光子弹，消灭迎面而来的小行星陨石与敌方飞船。
*   **游戏重置 (Restart)：** 本作未添加全局退出按钮。当玩家飞船被摧毁后，屏幕会弹出结束面板，点击 **`Restart` 按钮** 即可重新加载场景开始游戏。

### 🎓 教程来源 (Credits)
本项目在开发与还原过程中，参考了 B 站优秀主讲老师的实战教学课程：
*   **教程平台：** 哔哩哔哩 (Bilibili)
*   **主讲老师：** 齐齐课-Plane
*   **课程链接：** [【B站】Unity官方经典案例《SpaceShooter》实战教学课程](https://www.bilibili.com/video/BV1wW411N7Ew/?p=15)

---

## 📦 游戏美术资产导入教程 (Asset Import Tutorial)

为了方便大家导入本项目所使用的经典官方美术资源进行学习，我已经将打包好的 `.unitypackage` 资源包上传至本仓库中（文件名为 `SpaceShooterRes.unitypackage`）。

### 🛠️ 导入步骤：
1. **新建/打开项目：** 打开你的 Unity 2022 任意版本项目。
2. **选择导入包：** 在顶部菜单栏中选择 `Assets` $\rightarrow$ `Import Package` $\rightarrow$ `Custom Package...`。
3. **选中文件：** 在弹出的文件选择器中，选中你从本仓库下载的 `SpaceShooterRes.unitypackage`。
4. **全选导入：** 在弹出的 `Import Unity Package` 窗口中，确保勾选了左下角的 `All`（全选），然后点击右下角的 **`Import`** 按钮。
5. **开始创作：** 导入完成后，你就可以在你的 Project 窗口中看到完整的音效、材质、贴图及战机模型资源了。

---

## 🛠️ 核心技术回顾与 API 深度解析 (Technical Review)

在本项目中，我将整套游戏逻辑进行了模块化拆解，并在 XZ 物理平面的大框架下，通过编写自定义物理与生命周期脚本，重构了这一经典案例。以下是我在开发过程中学到的核心技术点与难点回顾：

### 1. 无缝滚动太空背景的制作
*   **场景构建：** 场景中的星空背景由两个水平平铺的 **`Quad`（四边形）** 组成，挂载在同一个空父物体下进行统一的层级管理。
*   **完美拼接原理：** 这两个四边形使用了完全相同的无缝拼接材质贴图。当它们沿着 Z 轴负方向（`-Vector3.forward`）移动时，在视觉上是完全看不出连接处的。
*   **代码控制 (`BGScroller.cs`)：**
    背景的循环往复移动由以下代码驱动：
    ```csharp
    float dis = Mathf.Repeat(ScrollSpeed * Time.time, 30);
    transform.position = startPos + dis * Vector3.forward * (-1);
    ```
    *   **`Mathf.Repeat(t, length)` 作用解析：** 这是一个极其好用的“跑回圈”函数。它能让传入的值（时间 $\times$ 速度）永远限制在 `0` 到 `30`（背景图单张宽度）之间。一旦数值达到 `30`，它会瞬间自动跳回 `0` 重新累加。因为两个相同的背景四边形首尾相接且间距正好为 30 米，所以这种“重置回圈”在视觉上完全实现了**无缝、无限循环的滚动背景**，玩家肉眼完全无法察觉。

### 2. 玩家飞船移动控制与物理平滑 (`PlayerShipController.cs`)
*   **刚体物理控制：** 为了保证物理计算的准确性，移动逻辑全部写在 **`FixedUpdate`** 物理帧循环中。通过 `Input.GetAxis` 获取 `Horizontal` 和 `Vertical` 虚拟轴输入，这极大地增强了不同硬件外设（手柄/键盘）的兼容性与可拓展性。
*   **飞船移动偏转（Tilt）：**
    为了让战机运动看起来不那么生硬，我让战机在左右移动时产生倾斜偏转：
    ```csharp
    rbd.rotation = Quaternion.Euler(0, 0, rbd.velocity.x * (-1) * tilt);
    ```
    *   **原理：** 根据刚体在 X 轴上的实时物理速度（`velocity.x`），反向计算出 Z 轴上的旋转角（EulerAngles），从而实现“往左移时飞船向左倾斜，往右移时向右倾斜”的动态物理质感。
*   **移动边界约束（Boundary）：**
    为了不让飞船飞出相机视野，定义了一个自定义类 `BoundDary` 来限定边界坐标。
    *   **`[System.Serializable]` 作用解析：** 在 C# 中，自定义的类默认是无法被 Unity 识别和存储的。在类上方贴上 `[System.Serializable]` 标签，相当于给 Unity 提供了**“反序列化说明书”**，告诉 Unity 这个结构可以被拆解保存，从而让 `Boundary` 的四个边界值（`xMin`, `xMax`, `zMin`, `zMax`）能**成功展现在 Inspector 属性面板上**供我们手动微调。
    *   **`Mathf.Clamp(value, min, max)` 限制原理：** 它将飞船的当前位置强制限制在 Min 和 Max 之间，一旦坐标超出边界值，会强行赋回边界极值，从而死死将飞船锁在屏幕内。
*   **子弹发射冷却计时器：**
    如果直接检测 `Input.GetButton("Fire1")`，由于每秒有几十帧，按一下会瞬间吐出几十颗子弹。为此我设计了一个**冷却时间计时器**：
    ```csharp
    if (Input.GetButton("Fire1") && nextShot < Time.time)
    {
        nextShot = Time.time + shotSpace;
        Instantiate(Bullet, shotPos.position, shotPos.rotation);
        // 播放音效...
    }
    ```
    *   通过 `Time.time`（当前游戏时间）加上 `shotSpace`（发射间隔）计算出下一次可以发射的绝对时间戳 `nextShot`。每次发射前进行大小判定，完美实现了非连续、有节奏的子弹发射频率。

### 3. 音效系统
*   每个音效均通过添加 `AudioSource` 组件来实现。
*   在编辑器中，务必**取消勾选 `Play On Awake`（防止物体一生成就突兀地播放声音）和 `Loop`（因为子弹发射和爆炸声都是单次播放）**，并在代码需要的时候手动调用 `.Play()` 方法播放（如子弹生成的瞬间）。

### 4. 随机敌机与小行星生成 (`GameMgr.cs`)
*   **协程（Coroutine）波次生成：**
    利用协程的非阻塞等待特性，通过 `IEnumerator` 和 `yield return new WaitForSeconds` 延迟指令，控制每一波敌人生成之间的空隙，并用布尔值 `isGameOver` 循环标记控制何时安全跳出循环。
*   **`Random.Range` 的 float 与 int 版本区别：**
    *   **整数版本 `Random.Range(0, Enemys.Length)`：** 属于**左闭右开区间**（不包含最大值）。这里能确保生成的随机索引值在 `0` 到 `Length - 1` 之间，绝对不会越界报错。
    *   **小数版本 `Random.Range(-5f, 5f)`：** 属于**闭区间**（包含最大值本身）。用于生成小行星在 X 轴上的随机出生坐标。
*   使用 `Instantiate` 进行物体在指定坐标和旋转下的实例化。

### 5. 子弹控制与场景自动清理
*   **子弹驱动 (`MoveController.cs`)：** 所有移动物体（包含子弹、陨石、敌人）的移动均统一由 `MoveController` 管理。通过刚体物理速度与 `transform.forward` 朝向相结合：`rbd.velocity = transform.forward * FlySpeed`，使物体向其前方飞去。
*   **场景清理 (`DestoryByKillBox.cs`)：**
    由于子弹和敌人超出屏幕后不会自动消失，如果不处理，场景中会有成千上万个无用物体导致游戏卡死。
    *   **解决方案：** 在屏幕视口外围摆放一个巨大的空物体，挂载 `Box Collider`（设为 `Is Trigger`）。
    *   当子弹或敌人飞出这个区域时，会自动触发 `OnTriggerExit(Collider other)` 回调，代码执行 `Destroy(other.gameObject)` 将其销毁，保证了内存和场景的绝对干净。

### 6. 敌机 AI、自动射击与平滑位移
*   **枪口空物体定位：** 在敌机模型前端创建一个空的子物体 `shotPos`，用它的 Transform 信息来定位子弹的生成位置，极大方便了枪口的模块化微调。
*   **`InvokeRepeating` 自动开火机制 (`EnemyWeaponController.cs`)：**
    在 `Start` 中调用：
    ```csharp
    InvokeRepeating("EnemyShipFire", shotWait, shotSpace);
    ```
    *   **作用解析：** 这是一个高效率的定时重复器。它会在 `shotWait` 秒后第一次调用 `"EnemyShipFire"` 方法，之后每隔 `shotSpace` 秒不间断地重复调用，从而实现了敌机的自动规律开火。
*   **敌机左右躲避 AI (`DodgeController.cs`)：**
    通过协程随机产生一个左右移动的目标速度（`dodgeTargetSpeed`），若战机偏左，则目标速度偏右，反之亦然。
*   **`Mathf.MoveTowards` 平滑渐变应用：**
    ```csharp
    float dodgeVal = Mathf.MoveTowards(rbd.velocity.x, dodgeTargetSpeed, Time.deltaTime * accelerSpeed);
    ```
    *   **作用解析：** 它是专门用来做数值平滑渐变的。它会让敌机当前的 X 轴物理速度，以最大每秒 `accelerSpeed` 的增量，平滑、稳定地逼近目标速度，**从而避免了敌机在左右闪避时发生瞬间的速度瞬移，让飞船的机动动作看起来极其自然、带有惯性。**
*   **小行星自然旋转 (`RoteController.cs`)：**
    ```csharp
    rbd.angularVelocity = Random.insideUnitSphere * rotSpeed;
    ```
    *   **`Random.insideUnitSphere` 作用解析：** 返回一个半径为 1 的球体内部的**随机三维向量（Vector3）**。因为这个向量的 X、Y、Z 轴方向和大小是完全随机且不规则的，乘以自转速度后赋给刚体的角速度（`angularVelocity`），**能完美让小行星产生完全不规则、纯天然的 3D 翻滚自转效果**，摆脱死板。

### 7. 碰撞逻辑与交互控制 (`ContactCheck.cs`)
*   所有会产生碰撞伤害的物体（小行星、敌机、敌子弹）都挂载了此脚本，通过 `OnTriggerEnter` 统一判定。
*   **避坑防自爆：** 由于游戏一运行，这些物体就会和外围的 `KillBox（Boundary）` 发生碰撞导致初始自爆。因此代码首行进行标签过滤：如果是 `Boundary`（边界）或 `Enemy`（同盟），直接 `return` 退出不处理。
*   **爆炸特效实例化与销毁顺序：**
    如果碰到玩家，实例化玩家爆炸特效，调用 `GameMgr` 的 `GameOver()`。
    *   **销毁顺序避坑：** 必须先执行 `Destroy(other.gameObject)` 销毁对方，再执行 `Destroy(gameObject)` 销毁自己。因为如果先销毁了自己，当前脚本在内存中就被释放了，下一行的 `other.gameObject` 就会丢失引用导致报错。

### 8. 游戏生命周期与 UI 逻辑 (`GameMgr.cs`)
*   在 `GameMgr` 全局管理器中定义 `AddScore` 刷新文本。
*   当玩家被摧毁，调用 `GameOver()` 激活结束面板，将 `isGameOver` 设为 `true` 停止协程生成敌人。
*   点击重启按钮，在 UI 按钮的 `OnClick()` 事件中订阅 `RestartGame()` 函数，调用 `SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex)` 重新加载当前场景实现重玩。

### 9. 爆炸特效自动清理销毁 (`DestoryByTime.cs`)
*   实例化的爆炸特效播放完毕后会作为垃圾留在场景中。为此给每个爆炸特效挂载 `DestoryByTime.cs`。
*   利用 `Destroy` 的重载版本：`Destroy(gameObject, delaytime)`，在延迟 `delaytime` 秒（特效播放完毕后）自动将特效本体从场景中安全销毁。

---

## 📝 个人开发感悟 (My Reflections)

1.  **数学函数的威力：** 
    整个游戏开发的难度并不算特别大，但有很多地方如果用传统条件判断写会非常冗长。通过使用 Unity 内置的数学函数（如 `Mathf.Repeat` 循环背景、`Mathf.Clamp` 限制边界、`Mathf.MoveTowards` 平滑速度），代码量被极大地简化，且运行效率极高。灵活运用物理与数学 API 是决定代码是否优雅的关键。
2.  **迭代补充与框架设计的重要性：** 
    整个制作过程就像是一个“滚雪球”的过程。我们先搭建最基础的移动，然后补充子弹，再补充敌人生成，最后加入闪避 AI、UI 面板和音效。在不断补充和完善的过程中，我深刻体会到了**优秀框架（Framework）的重要性**——好的框架拓展性和功能性会非常好。
3.  **全局控制器与层次结构管理的经验：**
    本项目使用一个空物体 `GameMgr` 作为全局控制器来掌管游戏的开始、结束和分数，这是一个非常经典的 Unity 架构思想。同时，学会将相同类型的粒子、环境物体在 Hierarchy 中挂载到空物体下进行分类管理，这对于大型项目场景的整洁度至关重要。这些看似简单的习惯，都是开发经验的宝贵积累。
4.  **审美与游戏素材：**
    本项目虽然省略了繁琐的美术素材制作和寻找过程，极大地节省了开发时间。但这也让我意识到，美术、音效等素材的搜集与审美能力，同样是独立游戏开发中不可或缺的硬实力。

---

## 🎨 推荐游戏开发资产与实用网站

在进行独立游戏制作时，素材和审美的积累非常关键。以下是我个人最近精心收藏的几个极为实用的游戏开发与美术制作网站，推荐给大家：

*   **[HoloPix 3D AI 贴图生成](https://holopix.cn)：** 强大的 AI 3D 贴图与纹理生成平台，是搜集和制作游戏材质、无缝背景贴图的极佳工具。
*   **[Qmai 奇脉骨骼动画](https://www.qmai.vip)：** 专为 2D 游戏开发者准备的骨骼动画制作与资源平台，对于快速制作精美的角色动画非常有帮助。

---

*感谢你的阅读！本项目的所有 C# 核心代码文件已同步上传至该仓库，欢迎大家下载、导入并一起交流学习！*