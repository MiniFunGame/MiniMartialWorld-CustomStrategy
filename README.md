# 计略（StrategyEffect）制作教程

> 本教程基于示例文件完整展示其源码并逐个解析功能点，帮助你快速编写并发布自定义计略。

## 目录
- [计略框架速览](#计略框架速览)
- [制作流程](#制作流程)
- [常用回调与要点](#常用回调与要点)
- [示例逐个解析](#示例逐个解析)

## 计略框架速览


- **典型声明**：`public class Xxx : StrategyEffect, IMultipleVariantEffect`
  - `IMultipleVariantEffect`：用于声明多个**品质（Quality）**变体（如 Common/Rare/Epic/Legendary），通过 `GetVariants()` 返回不同数值版本。
- **关键字段**：
  - `Name`（显示名）、`IsPostiveEffect`（正/负面）、`Quality`（品质）、`MinDifficulty`（最低难度）、`ChallengeNames` / `IsSpecialChallengeEffect`（特定挑战限定）、`Sort`（执行排序）。
- **常用监听**（按触发时机）：
  - 战斗开始：`OnInit()`
  - 回合开始/结束：`OnTurnStart() / OnTurnEnd()`
  - 受到伤害：`BeHit(ref int damage)`
  - 我方/对方使用技能：`ExcecuteSkill` / `OpponentExcecuteSkill`
  - 闪避触发：`OnDodge()`
  - 恢复触发：`Recover(ref int amount)`
  - 消耗道具：`ConsumeItem(Item item)`
  - 条件开启：`CheckEnable()`（返回 `true` 才能在谋划中合成出来）
  - 获得效果；`GainEffect(ref Effect effect)`

## 制作流程


1. **创建类**：新建 C# 脚本，定义 `public class MyEffect : StrategyEffect`（可选实现 `IMultipleVariantEffect`）。
2. **命名与正负性**：在构造函数里设置 `Name`、`IsPostiveEffect`、（可选）`Quality`。
3. **写效果**：根据需求重写相应回调（如 `OnInit`、`OnTurnEnd` 等），调用 `Loader` 上的属性修改接口（如 `EnhanceAttackByFixedValue`、`ModifyArmor`、`RecoverHealth` 等）。
4. **（可选）分品质**：实现 `IEnumerable<Effect> GetVariants()` 返回不同数值版本，供抽卡/掉落等系统根据品质选择。
5. **（可选）限定条件**：重写 `CheckEnable()`，用难度、挑战模式或角色标识限制生效范围。
6. **编译与放置**：将脚本加入你的 Mod/DLL 工程，编译为 DLL，放到游戏的创意工坊内容目录即可被加载。


## 示例解析

### GongRui

```csharp
// === 计略：攻锐（GongRui）===
// 类型：正面

[System.Serializable]
public class GongRui : StrategyEffect, IMultipleVariantEffect
{
    private int AttackIncrease = 25;

    public GongRui()
    {
        IsPostiveEffect = true;
        Name = "攻锐";
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"战斗开始时攻击力增加{AttackIncrease}";
    }

    // 触发：战斗开始；效果：提升攻击（固定值）

    public override void OnInit()
    {
        base.OnInit();
        Loader.EnhanceAttackByFixedValue(AttackIncrease);
    }

    public IEnumerable<Effect> GetVariants()
    {
        return new List<GongRui>
    {
        new GongRui { AttackIncrease = 25, Quality = Quality.Common },
        new GongRui { AttackIncrease = 40, Quality = Quality.Uncommon },
        new GongRui { AttackIncrease = 60, Quality = Quality.Rare },
        new GongRui { AttackIncrease = 90, Quality = Quality.Epic },
        new GongRui { AttackIncrease = 150, Quality = Quality.Legendary }
    };
    }

}
```

### JianShou

```csharp
// === 计略：坚守（JianShou）===
// 类型：正面

[System.Serializable]
public class JianShou : StrategyEffect, IMultipleVariantEffect
{
    private int DefenseIncrease = 20;

    public JianShou()
    {
        IsPostiveEffect = true;
        Name = "坚守";
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"战斗开始时防御力增加{DefenseIncrease}";
    }

    // 触发：战斗开始；效果：提升防御、提升防御（固定值）

    public override void OnInit()
    {
        base.OnInit();
        Loader.EnhanceDefenseByFixedValue(DefenseIncrease);
    }

    public IEnumerable<Effect> GetVariants()
    {
        return new List<JianShou>
        {
            new JianShou { DefenseIncrease = 20, Quality = Quality.Common },
            new JianShou { DefenseIncrease = 40, Quality = Quality.Uncommon },
            new JianShou { DefenseIncrease = 65, Quality = Quality.Rare },
            new JianShou { DefenseIncrease = 100, Quality = Quality.Epic },
            new JianShou { DefenseIncrease = 150, Quality = Quality.Legendary }
        };
    }
}
```

### QingJie

```csharp
// === 计略：轻捷（QingJie）===
// 类型：正面

[System.Serializable]
public class QingJie : StrategyEffect, IMultipleVariantEffect
{
    private float AgilityIncrease = 0.3f;

    public QingJie()
    {
        IsPostiveEffect = true;
        Name = "轻捷";
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"战斗开始时出手速度增加{AgilityIncrease:F1}";
    }

    // 触发：战斗开始；效果：提升速度/出手

    public override void OnInit()
    {
        base.OnInit();
        Loader.EnhanceAgilityByFixedValue(AgilityIncrease);
    }

    public IEnumerable<Effect> GetVariants()
    {
        return new List<QingJie>
    {
        new QingJie { AgilityIncrease = 0.3f, Quality = Quality.Common },
        new QingJie { AgilityIncrease = 0.4f, Quality = Quality.Uncommon },
        new QingJie { AgilityIncrease = 0.5f, Quality = Quality.Rare },
        new QingJie { AgilityIncrease = 0.6f, Quality = Quality.Epic },
        new QingJie { AgilityIncrease = 0.9f, Quality = Quality.Legendary }
    };
    }

}
```

### TuiFan

```csharp
// === 计略：蜕凡（TuiFan）===
// 类型：正面；默认品质：Rare
// 描述：所有白色品质技能的威力翻倍，内力消耗翻倍

[System.Serializable]
public class TuiFan : StrategyEffect
{

    // 构造函数
    public TuiFan()
    {
        IsPostiveEffect = true;
        Quality = Quality.Rare;
        Name = "蜕凡";
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return "所有白色品质技能的威力翻倍，内力消耗翻倍";
    }

    // 添加效果时的逻辑
    // 触发：战斗开始；效果：遍历技能修改、影响技能伤害倍率、影响技能内力消耗

    public override void OnInit()
    {
        base.OnInit();
        foreach (BasicSkill skill in Loader.Skills)
        {
            // Check if the skill's quality is "common"
            if (skill.Quality == Quality.Common) // Assuming `Quality.Common` represents the common quality
            {
                // Double the skill's power and inner power consumption
                skill.DamageRate *= 2;
                skill.ManaCost *= 2;
            }
        }

    }

}
```

### WangDao

```csharp
// === 计略：王道（WangDao）===
// 类型：正面
[System.Serializable]
public class WangDao : StrategyEffect, IMultipleVariantEffect
{
    private float damageIncreaseMultiplier = 1.2f; // 第一个技能威力提升倍率

    // 构造函数
    public WangDao()
    {
        IsPostiveEffect = true;
        Name = "王道";
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"第一个技能威力提升{(damageIncreaseMultiplier - 1) * 100:F0}%。";
    }

    // 添加效果时的逻辑
    // 触发：战斗开始；效果：遍历技能修改、影响技能伤害倍率

    public override void OnInit()
    {
        base.OnInit();
        if (Loader.Skills.Count > 0)
        {
            BasicSkill firstSkill = Loader.Skills[0] as BasicSkill;
            if (firstSkill != null)
            {
                firstSkill.DamageRate *= damageIncreaseMultiplier;
            }
        }
    }

    public IEnumerable<Effect> GetVariants()
    {
        // 返回不同参数的 WangDao 实例
        return new List<WangDao>
    {
        new WangDao { damageIncreaseMultiplier = 1.1f, Quality = Quality.Common },
        new WangDao { damageIncreaseMultiplier = 1.125f, Quality = Quality.Uncommon },
        new WangDao { damageIncreaseMultiplier = 1.15f, Quality = Quality.Rare },
        new WangDao { damageIncreaseMultiplier = 1.175f, Quality = Quality.Epic },
        new WangDao { damageIncreaseMultiplier = 1.25f, Quality = Quality.Legendary },
    };
    }

}
```

### FengChi

```csharp
// === 计略：风驰（FengChi）===
// 类型：正面；默认品质：Common
// 描述：所有技能的冷却时间减少1回合
[System.Serializable]
public class FengChi : StrategyEffect
{
    // 构造函数
    public FengChi()
    {
        IsPostiveEffect = true;
        Quality = Quality.Common;
        Name = "风驰";
        MinDifficulty = 0;
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return "所有技能的冷却时间减少1回合";
    }

    // 添加效果时的逻辑
    // 触发：战斗开始；效果：减少冷却、遍历技能修改

    public override void OnInit()
    {
        base.OnInit();
        foreach (Skill skill in Loader.Skills)
        {
            skill.ReduceCooldown(1); // Assuming a method to reduce cooldown exists
        }
    }
}
```

### LiBing

```csharp
// === 计略：利兵（LiBing）===
// 类型：正面

[System.Serializable]
public class LiBing : StrategyEffect, IMultipleVariantEffect
{
    private float damageIncrease = 0.07f; // 所有伤害类技能威力提升

    // 构造函数
    public LiBing()
    {
        IsPostiveEffect = true;
        Name = "利兵";
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"战斗开始时，所有伤害类技能威力增加{damageIncrease * 100:F0}。";
    }

    // 添加效果时的逻辑
    // 触发：战斗开始；效果：遍历技能修改、影响技能伤害倍率

    public override void OnInit()
    {
        base.OnInit();
        foreach (BasicSkill skill in Loader.Skills)
        {
            if (!skill.NoDamage)
            {
                skill.DamageRate += damageIncrease; // 按比例增加威力
            }
        }
    }

    public IEnumerable<Effect> GetVariants()
    {
        // 返回不同品质的 LiBing 实例
        return new List<LiBing>
        {
            new LiBing { damageIncrease = 0.20f, Quality = Quality.Common },    // 提升 7%
            new LiBing { damageIncrease = 0.25f, Quality = Quality.Uncommon }, // 提升 10%
            new LiBing { damageIncrease = 0.30f, Quality = Quality.Rare },     // 提升 12%
            new LiBing { damageIncrease = 0.40f, Quality = Quality.Epic },     // 提升 15%
            new LiBing { damageIncrease = 0.50f, Quality = Quality.Legendary } // 提升 20%
        };
    }
}
```

### GangGu

```csharp
// === 计略：钢骨（GangGu）===
// 类型：正面

[System.Serializable]
public class GangGu : StrategyEffect, IMultipleVariantEffect
{
    private float defenseMultiplier = 2.5f; // 默认提升比例
    private const float baseMultiplier = 2.0f; // 最低提升比例

    public GangGu()
    {
        Name = "钢骨";
        IsPostiveEffect = true;
    }
    // 触发：生效判定；效果：仅困难难度以以上才会出现

    public override bool CheckEnable()
    {
        return Controller.Save.DifficultyLevel > 1;
    }
    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"战斗开始时，自身防御提升(根骨x{defenseMultiplier})%";
    }

    // 触发：战斗开始；效果：提升防御、提升防御（乘算）

    public override void OnInit()
    {
        base.OnInit();

        // 获取根骨（Physique）属性值，并根据品质来设置提升比例
        float physique = Loader.Player.AttributePart.Physique;
        float defenseIncrease = physique * (defenseMultiplier / 100.0f);
        // 增加防御
        Loader.EnhanceDefenseByMultiply((int)defenseIncrease);
    }

    public IEnumerable<Effect> GetVariants()
    {
        return new List<GangGu>
        {
            new GangGu { defenseMultiplier = 2.5f, Quality = Quality.Legendary }, // 最高品质，提升2.5%
            new GangGu { defenseMultiplier = 2.25f, Quality = Quality.Epic },    // Epic品质，提升2.25%
            new GangGu { defenseMultiplier = 2.0f, Quality = Quality.Rare },     // Rare品质，提升2%
        };
    }
}
```

## BuffSample_2.cs

### YanShen

```csharp
// === 计略：岩身（YanShen）===
// 类型：正面

[System.Serializable]
public class YanShen : StrategyEffect, IMultipleVariantEffect
{
    private int skillCountMultiplier = 7; // 默认提升比例
    private int maxSkillCount = 3; // 最低的技能数提升比例

    public YanShen()
    {
        Name = "岩身";
        IsPostiveEffect = true;
    }
    // 触发：生效判定；效果：仅困难难度以以上才会出现

    public override bool CheckEnable()
    {
        return Controller.Save.DifficultyLevel > 1;
    }
    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"战斗开始时，防御提升(掌握招式数x{skillCountMultiplier})%";
    }

    // 触发：战斗开始；效果：提升防御、提升防御（乘算）

    public override void OnInit()
    {
        base.OnInit();

        // 获取掌握招式数，并计算防御提升
        int num = Loader.Player.Skills.Count;
        float defenseIncrease = num * (skillCountMultiplier / 100.0f);

        // 增加防御
        Loader.EnhanceDefenseByMultiply((int)defenseIncrease);
    }

    public IEnumerable<Effect> GetVariants()
    {
        return new List<YanShen>
        {
            new YanShen { skillCountMultiplier = 4, Quality = Quality.Legendary }, // 最高品质，提升7%
            new YanShen { skillCountMultiplier = 3, Quality = Quality.Epic },    // Epic品质，提升6%
            new YanShen { skillCountMultiplier = 2, Quality = Quality.Rare },    // Rare品质，提升5%
        };
    }
}
```

### NingShen_1

```csharp
// === 计略：凝身（NingShen_1）===
// 类型：正面
[System.Serializable]
public class NingShen_1 : StrategyEffect, IMultipleVariantEffect
{
    private float defenseMultiplier = 50f; // 默认防御提升百分比
    private float damageReductionRate = 10f; // 默认减少的防御提升比例

    public NingShen_1()
    {
        Name = "凝身";
        IsPostiveEffect = true;
    }
    // 触发：生效判定；效果：仅困难难度以以上才会出现

    public override bool CheckEnable()
    {
        return Controller.Save.DifficultyLevel > 1;
    }
    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"战斗开始时，防御提升{defenseMultiplier}%，受到伤害后提升效果降低{damageReductionRate}%";
    }

    // 触发：战斗开始；效果：提升防御、提升防御（乘算）

    public override void OnInit()
    {
        base.OnInit();

        // 增加防御
        Loader.EnhanceDefenseByMultiply(defenseMultiplier / 100); // 根据设定的百分比增加防御
    }

    // 触发：受到伤害后；效果：提升防御、提升防御（乘算）

    public override void BeHit(ref int num)
    {
        base.BeHit(ref num);

        // 每受到伤害，降低提升效果
        defenseMultiplier -= damageReductionRate;

        // 增加新的防御效果
        Loader.EnhanceDefenseByMultiply(-damageReductionRate / 100);

        // 限制最小值
        if (defenseMultiplier <= 0)
        {
            IsFinished = true;//为true后 程序会销毁这个Effect
        }
    }

    public IEnumerable<Effect> GetVariants()
    {
        return new List<NingShen_1>
        {
            new NingShen_1 { defenseMultiplier = 50f, damageReductionRate = 10f, Quality = Quality.Legendary }, // 最高品质，提升50%，减少10%
            new NingShen_1 { defenseMultiplier = 40f, damageReductionRate = 8f, Quality = Quality.Epic },     // Epic品质，提升40%，减少8%
            new NingShen_1 { defenseMultiplier = 30f, damageReductionRate = 6f, Quality = Quality.Rare },     // Rare品质，提升30%，减少6%
            new NingShen_1 { defenseMultiplier = 20f, damageReductionRate = 4f, Quality = Quality.Common },   // Common品质，提升20%，减少4%
        };
    }
}
```

### PoShi

```csharp
// === 计略：破势（PoShi）===
// 类型：正面
[System.Serializable]
public class PoShi : StrategyEffect, IMultipleVariantEffect
{
    private int damageIncrease = 10; // 默认威力提升10

    public PoShi()
    {
        Name = "破势";
        IsPostiveEffect = true;
    }
    // 触发：生效判定；效果：仅困难难度以以上才会出现

    public override bool CheckEnable()
    {
        return Controller.Save.DifficultyLevel > 1;
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"使用伤害类招式后，使其威力提升{damageIncrease}";
    }

    // 触发：释放技能时；效果：影响技能伤害倍率

    public override void ExcecuteSkill(ref Skill usedSkill)
    {
        base.ExcecuteSkill(ref usedSkill);

        if (usedSkill is BasicSkill && !((BasicSkill)usedSkill).NoDamage)
        {
            // 增加威力
            ((BasicSkill)usedSkill).DamageRate += damageIncrease / 100f;
        }
    }

    public IEnumerable<Effect> GetVariants()
    {
        return new List<PoShi>
        {
            new PoShi { damageIncrease = 6, Quality = Quality.Rare },
            new PoShi { damageIncrease = 8, Quality = Quality.Epic },
            new PoShi { damageIncrease = 10, Quality = Quality.Legendary },
        };
    }
}
```

### NiShi

```csharp
// === 计略：逆势（NiShi）===
// 类型：正面
[System.Serializable]
public class NiShi : StrategyEffect, IMultipleVariantEffect
{
    private float actionAdvance = 15f; // 默认行动提前15%

    public NiShi()
    {
        Name = "逆势";
        IsPostiveEffect = true;
    }
    // 触发：生效判定；效果：（见方法体）

    public override bool CheckEnable()
    {
        return Controller.Save.DifficultyLevel > 1;
    }
    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"受到伤害后，行动提前{actionAdvance}%";
    }

    // 触发：受到伤害后；效果：（见方法体）

    public override void BeHit(ref int num)
    {
        base.BeHit(ref num);

        Loader.ModifyProcess(actionAdvance / 100f);

    }

    public IEnumerable<Effect> GetVariants()
    {
        return new List<NiShi>
        {
            new NiShi { actionAdvance = 15f, Quality = Quality.Legendary },
            new NiShi { actionAdvance = 12.5f, Quality = Quality.Epic },
            new NiShi { actionAdvance = 10f, Quality = Quality.Rare },
        };
    }
}
```

### ZhuiYing

```csharp
// === 计略：追影（ZhuiYing）===
// 类型：正面
[System.Serializable]
public class ZhuiYing : StrategyEffect, IMultipleVariantEffect
{
    private float speedIncreaseRate = 0.12f; // 默认出手速度提升12%

    public ZhuiYing()
    {
        Name = "追影";
        IsPostiveEffect = true;
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"回合结束时，若自身出手速度低于对方，出手速度+{speedIncreaseRate}";
    }
    // 触发：生效判定；效果：（见方法体）

    public override bool CheckEnable()
    {
        return Controller.Save.DifficultyLevel > 1;
    }
    // 触发：回合结束；效果：提升速度/出手

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();

        // 如果自己的出手速度低于对方，则提升出手速度
        if (AttributeCalculator.GetBaseSpeed(Loader.Player) < AttributeCalculator.GetBaseSpeed(Loader.GetOpponent().Player))
        {
            Loader.EnhanceAgilityByFixedValue(speedIncreaseRate);
        }
    }

    public IEnumerable<Effect> GetVariants()
    {
        return new List<ZhuiYing>
        {
            new ZhuiYing { speedIncreaseRate = 0.12f, Quality = Quality.Legendary },
            new ZhuiYing { speedIncreaseRate = 0.10f, Quality = Quality.Epic },
            new ZhuiYing { speedIncreaseRate = 0.08f, Quality = Quality.Rare },
            new ZhuiYing { speedIncreaseRate = 0.06f, Quality = Quality.Common },
        };
    }
}
```

### JiSheng

```csharp
// === 计略：济生（JiSheng）===
// 类型：正面

[System.Serializable]
public class JiSheng : StrategyEffect, IMultipleVariantEffect
{
    private float healthRecoveryRate = 2f;  // 默认恢复比例
    private float medicineLevelMultiplier = 1.5f; // 默认医术等级提升比例

    public JiSheng()
    {
        Name = "济生";
        IsPostiveEffect = true;
    }
    // 触发：生效判定；效果：仅困难难度以以上才会出现

    public override bool CheckEnable()
    {
        return Controller.Save.DifficultyLevel > 1;
    }
    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"回合结束后，恢复({healthRecoveryRate}+医术等级x{medicineLevelMultiplier})%血量";
    }

    // 触发：回合结束；效果：恢复生命、阈值/上限机制

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();

        // 获取医术等级
        int level = Loader.Player.SkillList.Medicine.getLevel();
        // 根据医术等级和比例计算恢复血量
        float recoveryAmount = Loader.Player.AttributePart.getMaxHealth() * ((healthRecoveryRate + (level * medicineLevelMultiplier)) / 100f);
        Loader.RecoverHealth((int)recoveryAmount);
    }

    public IEnumerable<Effect> GetVariants()
    {
        return new List<JiSheng>
        {
            new JiSheng { healthRecoveryRate = 2f, medicineLevelMultiplier = 1.5f, Quality = Quality.Legendary },
            new JiSheng { healthRecoveryRate = 2f, medicineLevelMultiplier = 1.25f, Quality = Quality.Epic },
            new JiSheng { healthRecoveryRate = 2f, medicineLevelMultiplier = 1f, Quality = Quality.Rare },
        };
    }
}
```

### KongMing

```csharp
// === 计略：空明（KongMing）===
// 类型：正面

[System.Serializable]
public class KongMing : StrategyEffect
{
    private int swordIntentIncrease = 2; // 默认增加的剑意层数

    public KongMing()
    {
        Name = "空明";
        IsPostiveEffect = true;
    }
    // 触发：生效判定；效果：仅困难难度以以上才会出现

    public override bool CheckEnable()
    {
        return Controller.Save.DifficultyLevel > 1;
    }
    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"调息时获得{ swordIntentIncrease}层【剑意】";
    }

    // 触发：释放技能时；效果：（见方法体）

    public override void ExcecuteSkill(ref Skill usedSkill)
    {
        base.ExcecuteSkill(ref usedSkill);

        // 检查技能名称是否为“调息”
        if (usedSkill.Name == "调息")
        {
            SwordForceControl.ModifyForce(Loader, swordIntentIncrease); // 增加剑意
        }
    }
}
```

### XunFeng

```csharp
// === 计略：迅锋（XunFeng）===
// 类型：正面；默认品质：Rare
// 描述：回合结束时，自身招式若有描述中包含“剑”字的，获得一层【剑意】
[System.Serializable]
public class XunFeng : StrategyEffect
{
    public XunFeng()
    {
        Name = "迅锋";
        IsPostiveEffect = true;
        Quality = Quality.Rare;
    }
    // 触发：生效判定；效果：仅困难难度以以上才会出现

    public override bool CheckEnable()
    {
        return Controller.Save.DifficultyLevel > 1;
    }
    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"回合结束时，自身招式若有描述中包含“剑”字的，获得一层【剑意】";
    }

    // 触发：回合结束；效果：遍历技能修改

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();

        // 检查所有招式的描述中是否包含"剑"
        foreach (var skill in Loader.Skills)
        {
            if (skill.getDiscription().Contains("剑"))
            {
                // 增加一层剑意
                SwordForceControl.ModifyForce(Loader, 1);
                break; // 如果至少有一个招式符合条件，就结束循环
            }
        }
    }
}
```

### NingGang

```csharp
// === 计略：墨守（NingGang）===
// 类型：正面

[System.Serializable]
public class NingGang : StrategyEffect, IMultipleVariantEffect
{
    private float shieldPercentage = 8f;  // 默认恢复量百分比作为护盾
    private float minShieldPercentage = 5f; // 最低护盾百分比

    public NingGang()
    {
        Name = "墨守";
        IsPostiveEffect = true;
        Sort = 2;
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"恢复血量时，获得回复量{shieldPercentage}%（受根骨影响）的护盾";
    }

    // 触发：进行恢复时；效果：获得护盾

    public override void Recover(ref int amount)
    {
        base.Recover(ref amount);

        // 计算护盾值，护盾量为恢复量的比例
        float shieldAmount = amount * ((shieldPercentage + Loader.Player.AttributePart.Physique) / 100f);

        // 给角色增加护盾
        Loader.ModifyArmor((int)shieldAmount);
    }

    public IEnumerable<Effect> GetVariants()
    {
        return new List<NingGang>
        {
            new NingGang { shieldPercentage = 18f, Quality = Quality.Legendary }, // 最高品质，护盾比例8%
            new NingGang { shieldPercentage = 15f, Quality = Quality.Epic },     // Epic品质，护盾比例7%
            new NingGang { shieldPercentage = 12f, Quality = Quality.Rare },     // Rare品质，护盾比例6%
            new NingGang { shieldPercentage = 10f, Quality = Quality.Common },   // Common品质，护盾比例5%
        };
    }
}
```

## BuffSample_3.cs

### LiuYingEffect

```csharp
// === 计略：流萤（LiuYingEffect）===
// 类型：正面

[System.Serializable]
public class LiuYingEffect : StrategyEffect, IMultipleVariantEffect
{
    private int totalHealthRecovered = 3000; // 每累计恢复的血量
    private int accumulatedHealth = 0; // 累计恢复的血量

    public LiuYingEffect()
    {
        Name = "流萤";
        IsPostiveEffect = true;
        //只有无尽模式和白骨生花模式的特定计略
        ChallengeNames = new List<string>() { HopeForImmortalControl.GetChallengeModeName(), EndlessModeControl.GetChallengeModeName() };
        // 用于判断是否是特定的挑战模式效果
        IsSpecialChallengeEffect = true;
        //优先级置于1，会在所有效果监听中较后执行（默认为0）
        Sort = 1;
        
    }

    // 触发：生效判定；效果：只有Identifier == "QIANKUNZHENXIE"或者 【白骨生花】模式才会出现

    public override bool CheckEnable()
    {
        if ((Controller.GameData.Player.Identifier == "QIANKUNZHENXIE") || HopeForImmortalControl.IsImmortalChallenge())
        {
            return true;
        }
        return false;
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"每累计恢复{totalHealthRecovered}点血量，出手速度+0.1";
    }

    // 触发：进行恢复时；效果：提升速度/出手

    public override void Recover(ref int amount)
    {
        base.Recover(ref amount);

        accumulatedHealth += amount;

        // 计算可以增加多少次出手速度
        int speedIncreaseCount = accumulatedHealth / totalHealthRecovered;

        if (speedIncreaseCount > 0)
        {
            accumulatedHealth -= speedIncreaseCount * totalHealthRecovered; // 减去已经用于增加出手速度的部分
            Loader.EnhanceAgilityByFixedValue(speedIncreaseCount * 0.1f); // 一次性增加出手速度
        }
    }

    public IEnumerable<Effect> GetVariants()
    {
        return new List<LiuYingEffect>
        {
            new LiuYingEffect { totalHealthRecovered = 2000, Quality = Quality.Legendary },
            new LiuYingEffect { totalHealthRecovered = 2500, Quality = Quality.Epic },
            new LiuYingEffect { totalHealthRecovered = 3000, Quality = Quality.Rare },
        };
    }
}
```

### GuBen

```csharp
// === 计略：固本（GuBen）===
// 类型：正面

[System.Serializable]
public class GuBen : StrategyEffect, IMultipleVariantEffect
{
    private float healthIncreaseRate = 0.40f; // 战斗开始时血量上限增加的比例

    public GuBen()
    {
        Name = "固本";
        IsPostiveEffect = true;
         //只有无尽模式和白骨生花模式的特定计略
        ChallengeNames = new List<string>() { HopeForImmortalControl.GetChallengeModeName(), EndlessModeControl.GetChallengeModeName() };
        // 用于判断是否是特定的挑战模式效果
        IsSpecialChallengeEffect = true;
    }
      // 触发：生效判定；效果：只有Identifier == "QIANKUNZHENXIE"或者 【白骨生花】模式才会出现

    public override bool CheckEnable()
    {
        if ((Controller.GameData.Player.Identifier == "QIANKUNZHENXIE") || HopeForImmortalControl.IsImmortalChallenge())
        {
            return true;
        }
        return false;
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"战斗开始时，血量上限提升{healthIncreaseRate * 100}%";
    }

    // 触发：战斗开始；效果：提高最大生命（乘算）、阈值/上限机制

    public override void OnInit()
    {
        Loader.EnhanceMaxHealthByMultiply(healthIncreaseRate);
    }

    public IEnumerable<Effect> GetVariants()
    {
        return new List<GuBen>
        {
            new GuBen { healthIncreaseRate = 0.40f, Quality = Quality.Legendary },
            new GuBen { healthIncreaseRate = 0.30f, Quality = Quality.Epic },
            new GuBen { healthIncreaseRate = 0.25f, Quality = Quality.Rare },
        };
    }
}
```

### XuYuan

```csharp
// === 计略：蓄元（XuYuan）===
// 类型：正面

[System.Serializable]
public class XuYuan : StrategyEffect, IMultipleVariantEffect
{
    private float healthIncreaseRate = 0.08f; // 每回合增加的血量上限比例
    private int maxTriggers = 7; // 最大触发次数
    private int triggerCount = 0; // 当前触发次数

    public XuYuan()
    {
        Name = "蓄元";
        IsPostiveEffect = true;
         //只有无尽模式和白骨生花模式的特定计略
        ChallengeNames = new List<string>() { HopeForImmortalControl.GetChallengeModeName(), EndlessModeControl.GetChallengeModeName() };
        // 用于判断是否是特定的挑战模式效果
        IsSpecialChallengeEffect = true;
    }
    // 触发：生效判定；效果：只有Identifier == "QIANKUNZHENXIE"或者 【白骨生花】模式才会出现

    public override bool CheckEnable()
    {
        if ((Controller.GameData.Player.Identifier == "QIANKUNZHENXIE") || HopeForImmortalControl.IsImmortalChallenge())
        {
            return true;
        }
        return false;
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"回合结束时，血量上限+{healthIncreaseRate * 100}%，最多触发{maxTriggers}次";
    }

    // 触发：回合结束；效果：提高最大生命（乘算）、阈值/上限机制

    public override void OnTurnEnd()
    {
        if (triggerCount < maxTriggers)
        {
            Loader.EnhanceMaxHealthByMultiply(healthIncreaseRate);
            triggerCount++;
        }
    }

    public IEnumerable<Effect> GetVariants()
    {
        return new List<XuYuan>
        {
            new XuYuan { maxTriggers = 7, Quality = Quality.Legendary },
            new XuYuan { maxTriggers = 6, Quality = Quality.Epic },
            new XuYuan { maxTriggers = 5, Quality = Quality.Rare },
        };
    }
}
```

### TieYi

```csharp
// === 计略：铁衣（TieYi）===
// 类型：正面

[System.Serializable]
public class TieYi : StrategyEffect, IMultipleVariantEffect
{
    private float shieldPercentage = 0.75f;  // 默认初始护盾百分比 75%

    public TieYi()
    {
        Name = "铁衣";
        IsPostiveEffect = true;
        //只有无尽模式和白骨生花模式的特定计略
        ChallengeNames = new List<string>() { HopeForImmortalControl.GetChallengeModeName(), EndlessModeControl.GetChallengeModeName() };
        // 用于判断是否是特定的挑战模式效果
        IsSpecialChallengeEffect = true;
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"战斗开始时，获得等于角色血量{shieldPercentage * 100}%的护盾";
    }

    // 触发：生效判定；效果：（见方法体）

    public override bool CheckEnable()
    {
        if ((Controller.GameData.Player.Identifier == "QIANKUNZHENXIE") || HopeForImmortalControl.IsImmortalChallenge())
        {
            return true;
        }
        return false;
    }

    // 触发：战斗开始；效果：获得/消耗护盾、阈值/上限机制

    public override void OnInit()
    {
        // 假设角色当前血量是Loader.GetCurrentHP()，具体的HP获取方式依据游戏框架可能需要调整
        float currentHP = Loader.Player.AttributePart.getMaxHealth();
        float shieldAmount = currentHP * shieldPercentage;
        Loader.ModifyArmor((int)shieldAmount);  // 假设Loader有一个GainShield方法来为角色增加护盾
    }

    public IEnumerable<Effect> GetVariants()
    {
        return new List<TieYi>
        {
            new TieYi { shieldPercentage = 0.40f, Quality = Quality.Legendary },  // 75%的护盾，最高品质
            new TieYi { shieldPercentage = 0.35f, Quality = Quality.Epic },      // 55%的护盾
            new TieYi { shieldPercentage = 0.20f, Quality = Quality.Rare },      // 40%的护盾，最低品质
        };
    }
}
```

### PoXiaoSword

```csharp
// === 计略：破晓（PoXiaoSword）===
// 类型：正面

[System.Serializable]
public class PoXiaoSword : StrategyEffect, IMultipleVariantEffect
{
    private int requiredSwordIntent = 30; // 需要的剑意层数
    private int damagePower = 250; // 触发时的基础威力

    public PoXiaoSword()
    {
        Name = "破晓";
        IsPostiveEffect = true;
        MinDifficulty = 1;
        Intensity = 0;
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"每累计获得{requiredSwordIntent}层剑意后，造成一次威力{damagePower}的伤害（必定命中，必定暴击）";
    }

    // 触发：GainEffect；效果：（见方法体）

    public override void GainEffect(ref Effect effect)
    {
        base.GainEffect(ref effect);

        // 检查是否为剑意效果，且强度大于 0
        if (effect != null && effect.Name == "剑意" && effect.Intensity > 0)
        {
            // 增加进度
            Intensity += effect.Intensity;

            // 检查是否达到触发条件
            if (Intensity >= requiredSwordIntent)
            {
                Intensity -= requiredSwordIntent;
                // 触发一次性伤害
                Loader.attack(damagePower * 0.01f, true, true); // 必定命中且必定暴击

                // 显示触发信息（可选

            }
        }
    }

    public IEnumerable<Effect> GetVariants()
    {
        // 返回不同参数的 PoXiao 实例
        return new List<PoXiaoSword>
        {
            new PoXiaoSword { damagePower = 450, Quality = Quality.Common },
            new PoXiaoSword { damagePower = 500, Quality = Quality.Uncommon },
            new PoXiaoSword { damagePower = 550, Quality = Quality.Rare },
            new PoXiaoSword { damagePower = 600, Quality = Quality.Epic },
            new PoXiaoSword { damagePower = 700, Quality = Quality.Legendary },
        };
    }
}
```

### JinGuiYaoLue

```csharp
// === 计略：金匮要略（JinGuiYaoLue）===
[System.Serializable]
public class JinGuiYaoLue : StrategyEffect, IMultipleVariantEffect
{
    // 每次恢复时增加的恢复效果百分比
    private float RecoveryIncreasePercentage = 0.25f;

    // 触发：进行恢复时；效果：（见方法体）

    public override void Recover(ref int num)
    {

        // 计算增加的恢复量，基于原始的恢复量
        int additionalRecovery = (int)(num * RecoveryIncreasePercentage);
        // 应用增加的恢复量
        num += additionalRecovery;
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        // 提供效果的描述
        return $"治疗效果提升{RecoveryIncreasePercentage * 100:F0}%。";
    }

    // 构造函数
    public JinGuiYaoLue()
    {
        Name = "金匮要略";
    }

    public IEnumerable<Effect> GetVariants()
    {
        // 返回不同参数的 JinGuiYaoLue 实例
        var list = new List<JinGuiYaoLue>()
        {
            new JinGuiYaoLue() { RecoveryIncreasePercentage = 0.15f, Name = "金匮要略", Quality = Quality.Uncommon },
            new JinGuiYaoLue() { RecoveryIncreasePercentage = 0.18f, Name = "金匮要略", Quality = Quality.Rare },
            new JinGuiYaoLue() { RecoveryIncreasePercentage = 0.20f, Name = "金匮要略", Quality = Quality.Epic },
        };
        return list;
    }
}
```

### FuQuShiGu

```csharp
// === 计略：跗蛆蚀骨（FuQuShiGu）===
// 类型：负面；默认品质：Legendary
// 描述：敌人使用招式后，使该招式内力消耗+10%
[System.Serializable]
public class FuQuShiGu : StrategyEffect
{
    public FuQuShiGu()
    {
        Name = "跗蛆蚀骨";
        IsPostiveEffect = false;
        Quality = Quality.Legendary;
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"敌人使用招式后，使该招式内力消耗+10%";
    }

    // 触发：敌方释放技能时；效果：影响技能内力消耗

    public override void OpponentExcecuteSkill(ref Skill skill)
    {
        base.OpponentExcecuteSkill(ref skill);
        skill.ManaCost = (int)(skill.ManaCost * 1.1f);
    }
}
```

### ZhuiFengCheDian

```csharp
// === 计略：追风掣电（ZhuiFengCheDian）===
// 类型：正面；默认品质：Legendary

[System.Serializable]
public class ZhuiFengCheDian : StrategyEffect
{
    private int param;

    public ZhuiFengCheDian(int param)
    {
        Name = "追风掣电";
        this.param = param;
        IsPostiveEffect = true;
        Quality = Quality.Legendary;
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"敌人回合开始时，自身行动提前{20 + 10 * param}%";
    }

    // 触发：OpponentOnTurnStart；效果：（见方法体）

    public override void OpponentOnTurnStart()
    {
        base.OpponentOnTurnStart();
        Loader.ModifyProcess((20 + 10 * param) / 100f);
    }

    [System.Serializable]
    public class YeHuoFenTian : StrategyEffect
    {
        private int accumulatedDamage = 300;

        public YeHuoFenTian()
        {
            Name = "业火焚天";
            IsPostiveEffect = false;
            Quality = Quality.Legendary;
        }

        // 触发：描述文本；效果：（见方法体）

        public override string getDiscription()
        {
            return $"拥有额外一条命，回合开始时扣除双方{accumulatedDamage}点血量，并强化此效果";
        }

        // 触发：战斗开始；效果：（见方法体）

        public override void OnInit()
        {
            base.OnInit();
            Loader.ExtraLife++;
        }

        // 触发：回合开始；效果：（见方法体）

        public override void OnTurnStart()
        {
            base.OnTurnStart();
            Loader.GetHurt(accumulatedDamage);
            Loader.GetOpponent().GetHurt(accumulatedDamage);
            accumulatedDamage += 50;  // 每回合强化
        }
    }
}
```

### YeHuoFenTian

```csharp
// === 计略：业火焚天（YeHuoFenTian）===
// 类型：负面；默认品质：Legendary

    [System.Serializable]
    public class YeHuoFenTian : StrategyEffect
    {
        private int accumulatedDamage = 300;

        public YeHuoFenTian()
        {
            Name = "业火焚天";
            IsPostiveEffect = false;
            Quality = Quality.Legendary;
        }

        // 触发：描述文本；效果：（见方法体）

        public override string getDiscription()
        {
            return $"拥有额外一条命，回合开始时扣除双方{accumulatedDamage}点血量，并强化此效果";
        }

        // 触发：战斗开始；效果：获得额外一条命

        public override void OnInit()
        {
            base.OnInit();
            Loader.ExtraLife++;
        }

        // 触发：回合开始；效果：（见方法体）

        public override void OnTurnStart()
        {
            base.OnTurnStart();
            Loader.GetHurt(accumulatedDamage);
            Loader.GetOpponent().GetHurt(accumulatedDamage);
            accumulatedDamage += 50;  // 每回合强化
        }
    }
```

### FuQi

```csharp
// === 计略：复气（FuQi）===
// 类型：正面

[System.Serializable]
public class FuQi : StrategyEffect, IMultipleVariantEffect
{
    private int additionalManaRecovery = 80; // 额外恢复的内力

    public FuQi()
    {
        IsPostiveEffect = true;
        Name = "复气";
        MinDifficulty = 1;
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"使用道具后，额外恢复{additionalManaRecovery}点内力。";
    }

    // 触发：消耗道具时；效果：恢复内力

    public override void ConsumeItem(Item item)
    {
        base.ConsumeItem(item);

        // 恢复额外的内力
        Loader.RecoverMana(additionalManaRecovery);
    }

    public IEnumerable<Effect> GetVariants()
    {
        // 三种变体：恢复 80 / 125 / 320 内力
        return new List<FuQi>
        {
            new FuQi { additionalManaRecovery = 160, Quality = Quality.Common },
            new FuQi { additionalManaRecovery = 300, Quality = Quality.Uncommon },
            new FuQi { additionalManaRecovery = 600, Quality = Quality.Rare },
            new FuQi { additionalManaRecovery = 1000, Quality = Quality.Epic },
            new FuQi { additionalManaRecovery = 1600, Quality = Quality.Legendary},
        };
    }
}
```

### TaHuaZhuiYueEffect

```csharp
// === 计略：踏花追月（TaHuaZhuiYueEffect）===
// 默认品质：Legendary

[System.Serializable]
public class TaHuaZhuiYueEffect : StrategyEffect
{
    private float healthRecoveryRate = 0.25f; // 默认每次闪避时恢复25%的闪避值

    // 不带参数的构造函数
    public TaHuaZhuiYueEffect() : this(0.5f) { }

    // 构造函数
    public TaHuaZhuiYueEffect(float healthRecoveryRate)
    {
        Quality = Quality.Legendary;
        Name = "踏花追月";
        this.healthRecoveryRate = healthRecoveryRate;
        SpriteName = "飞燕";
    }

    // 触发：描述文本；效果：（见方法体）

    public override string getDiscription()
    {
        return $"每回合结束时获得1层【轻身】，闪避时恢复当前闪避 {healthRecoveryRate * 100}%的生命值 ";
    }


    // 每次闪避时触发
    // 触发：闪避时；效果：恢复生命

    public override void OnDodge()
    {
        base.OnDodge();
        int recoveryAmount = (int)(AttributeCalculator.GetBaseEvasion(Loader.Player) * healthRecoveryRate);
        Loader.RecoverHealth(recoveryAmount);
    }

    // 每回合结束时触发
    // 触发：回合结束；效果：（见方法体）

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        Loader.AddEffect(new QingShenEffect(1)); // 每回合结束时获得 1 层【轻身】

    }
}
```


 