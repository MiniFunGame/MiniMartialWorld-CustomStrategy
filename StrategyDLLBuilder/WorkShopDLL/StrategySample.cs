using System;

[System.Serializable]
public class GuiYiEffect : StrategyEffect
{
    private const float advanceRate = 0.60f;
    private int count = 0;
    private const int maxCount = 5;

    public GuiYiEffect()
    {
        IsPostiveEffect = true;
        Quality = Quality.Legendary;
        Name = "归一";
    }

    public override string getDiscription()
    {
        return $"每次使用调息时，行动提前{advanceRate * 100:F0}%，最多{maxCount}次（已触发{count}）。";
    }

    public override void ExcecuteSkill(ref Skill skill)
    {
        base.ExcecuteSkill(ref skill);
        if (IsFinished) return;
        if (skill.Name == "调息")
        {
            Loader.CD += advanceRate; // 参照“剑影”的行动推进
            count++;
            if (count >= maxCount) IsFinished = true;
        }
    }
}

[System.Serializable]
public class XingNianEffect : StrategyEffect
{
    private int count = 0;
    private const int maxCount = 3;

    public XingNianEffect()
    {
        IsPostiveEffect = true;
        Quality = Quality.Legendary;
        Name = "静念";
    }

    public override string getDiscription()
    {
        return $"每次使用调息时，获得1层【抵御】，最多{maxCount}次（已触发{count}）。";
    }

    public override void ExcecuteSkill(ref Skill skill)
    {
        base.ExcecuteSkill(ref skill);
        if (IsFinished) return;
        if (skill.Name == "调息")
        {
            Loader.AddEffect(new DiYu(1)); // 项目里抵御的标准用法
            count++;
            if (count >= maxCount) IsFinished = true;
        }
    }
}

[System.Serializable]
public class HuanXiEffect : StrategyEffect
{
    private const float healRate = 0.30f;
    private int count = 0;
    private const int maxCount = 4;

    public HuanXiEffect()
    {
        IsPostiveEffect = true;
        Quality = Quality.Legendary;
        Name = "缓息";
    }

    public override string getDiscription()
    {
        return $"每次使用调息时，恢复{healRate * 100:F0}%血量，最多{maxCount}次（已触发{count}）。";
    }

    public override void ExcecuteSkill(ref Skill skill)
    {
        base.ExcecuteSkill(ref skill);
        if (IsFinished) return;
        if (skill.Name == "调息")
        {
            int maxHp = Loader.Player.AttributePart.getMaxHealth();
            Loader.RecoverHealth((int)(maxHp * healRate)); // 按上限比例回血
            count++;
            if (count >= maxCount) IsFinished = true;
        }
    }
}

[System.Serializable]
public class SheXinEffect : StrategyEffect
{
    private const float defUp = 0.20f;
    private int count = 0;
    private const int maxCount = 4;

    public SheXinEffect()
    {
        IsPostiveEffect = true;
        Quality = Quality.Legendary;
        Name = "摄心";
    }

    public override string getDiscription()
    {
        return $"每次使用调息时，防御提升{defUp * 100:F0}%（可叠加），最多{maxCount}次（已触发{count}）。";
    }

    public override void ExcecuteSkill(ref Skill skill)
    {
        base.ExcecuteSkill(ref skill);
        if (IsFinished) return;
        if (skill.Name == "调息")
        {
            Loader.EnhanceDefenseByMultiply(defUp); // 乘算叠加
            count++;
            if (count >= maxCount) IsFinished = true;
        }
    }
}

[System.Serializable]
public class XingGongEffect : StrategyEffect
{
    private const float advanceRate = 0.60f;
    private int count = 0;
    private const int maxCount = 5;

    public XingGongEffect()
    {
        IsPostiveEffect = true;
        Quality = Quality.Legendary;
        Name = "行功";
    }

    public override string getDiscription()
    {
        return $"每次使用调息时，行动提前{advanceRate * 100:F0}%，最多{maxCount}次（已触发{count}）。";
    }

    public override void ExcecuteSkill(ref Skill skill)
    {
        base.ExcecuteSkill(ref skill);
        if (IsFinished) return;
        if (skill.Name == "调息")
        {
            Loader.CD += advanceRate;
            count++;
            if (count >= maxCount) IsFinished = true;
        }
    }
}

[System.Serializable]
public class XiuQiEffect : StrategyEffect
{
    private const float healRate = 0.30f;
    private int count = 0;
    private const int maxCount = 4;

    public XiuQiEffect()
    {
        IsPostiveEffect = true;
        Quality = Quality.Legendary;
        Name = "修气";
    }

    public override string getDiscription()
    {
        return $"每次使用调息时，恢复{healRate * 100:F0}%血量，最多{maxCount}次（已触发{count}）。";
    }

    public override void ExcecuteSkill(ref Skill skill)
    {
        base.ExcecuteSkill(ref skill);
        if (IsFinished) return;
        if (skill.Name == "调息")
        {
            int maxHp = Loader.Player.AttributePart.getMaxHealth();
            Loader.RecoverHealth((int)(maxHp * healRate));
            count++;
            if (count >= maxCount) IsFinished = true;
        }
    }
}

[System.Serializable]
public class HuMaiEffect : StrategyEffect
{
    private const float defUp = 0.20f;
    private int count = 0;
    private const int maxCount = 4;

    public HuMaiEffect()
    {
        IsPostiveEffect = true;
        Quality = Quality.Legendary;
        Name = "护脉";
    }

    public override string getDiscription()
    {
        return $"每次使用调息时，防御提升{defUp * 100:F0}%（可叠加），最多{maxCount}次（已触发{count}）。";
    }

    public override void ExcecuteSkill(ref Skill skill)
    {
        base.ExcecuteSkill(ref skill);
        if (IsFinished) return;
        if (skill.Name == "调息")
        {
            Loader.EnhanceDefenseByMultiply(defUp);
            count++;
            if (count >= maxCount) IsFinished = true;
        }
    }
}

[System.Serializable]
public class XingMaiEffect : StrategyEffect
{
    private const float atkUp = 0.10f;
    private int count = 0;
    private const int maxCount = 5;

    public XingMaiEffect()
    {
        IsPostiveEffect = true;
        Quality = Quality.Legendary;
        Name = "醒脉";
    }

    public override string getDiscription()
    {
        return $"每次使用调息时，攻击提升{atkUp * 100:F0}%（可叠加），最多{maxCount}次（已触发{count}）。";
    }

    public override void ExcecuteSkill(ref Skill skill)
    {
        base.ExcecuteSkill(ref skill);
        if (IsFinished) return;
        if (skill.Name == "调息")
        {
            Loader.EnhanceAttackByMultiply(atkUp);
            count++;
            if (count >= maxCount) IsFinished = true;
        }
    }
}

[System.Serializable]
public class HeHunEffect : StrategyEffect
{
    private const float atkUp = 0.10f;
    private int count = 0;
    private const int maxCount = 5;

    public HeHunEffect()
    {
        IsPostiveEffect = true;
        Quality = Quality.Legendary;
        Name = "和魂";
    }

    public override string getDiscription()
    {
        return $"每次使用调息时，攻击提升{atkUp * 100:F0}%（可叠加），最多{maxCount}次（已触发{count}）。";
    }

    public override void ExcecuteSkill(ref Skill skill)
    {
        base.ExcecuteSkill(ref skill);
        if (IsFinished) return;
        if (skill.Name == "调息")
        {
            Loader.EnhanceAttackByMultiply(atkUp);
            count++;
            if (count >= maxCount) IsFinished = true;
        }
    }
}

[System.Serializable]
public class HuShiEffect : StrategyEffect
{
    private int count = 0;
    private const int maxCount = 3;

    public HuShiEffect()
    {
        IsPostiveEffect = true;
        Quality = Quality.Legendary;
        Name = "护识";
    }

    public override string getDiscription()
    {
        return $"每次使用调息时，获得1层【抵御】，最多{maxCount}次（已触发{count}）。";
    }

    public override void ExcecuteSkill(ref Skill skill)
    {
        base.ExcecuteSkill(ref skill);
        if (IsFinished) return;
        if (skill.Name == "调息")
        {
            Loader.AddEffect(new DiYu(1));
            count++;
            if (count >= maxCount) IsFinished = true;
        }
    }
}
