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
            Loader.CD += advanceRate;
            count++;
            if (count >= maxCount) IsFinished = true;
        }
    }
}

[System.Serializable]
public class XingGongEffect : StrategyEffect
{
    private const float advanceRate = 0.30f;

    public XingGongEffect()
    {
        IsPostiveEffect = true;
        Quality = Quality.Epic;
        Name = "行功";
    }

    public override string getDiscription()
    {
        return $"每次使用调息时，行动提前{advanceRate * 100:F0}%。";
    }

    public override void ExcecuteSkill(ref Skill skill)
    {
        base.ExcecuteSkill(ref skill);
        if (skill.Name == "调息")
        {
            Loader.CD += advanceRate;
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
        return $"每次使用调息时，获得1层抵御，最多{maxCount}次（已触发{count}）。";
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
            Loader.RecoverHealth((int)(maxHp * healRate));
            count++;
            if (count >= maxCount) IsFinished = true;
        }
    }
}

[System.Serializable]
public class XiuQiEffect : StrategyEffect
{
    private const float healRate = 0.10f;

    public XiuQiEffect()
    {
        IsPostiveEffect = true;
        Quality = Quality.Epic;
        Name = "修气";
    }

    public override string getDiscription()
    {
        return $"每次使用调息时，恢复{healRate * 100:F0}%血量。";
    }

    public override void ExcecuteSkill(ref Skill skill)
    {
        base.ExcecuteSkill(ref skill);
        if (skill.Name == "调息")
        {
            int maxHp = Loader.Player.AttributePart.getMaxHealth();
            Loader.RecoverHealth((int)(maxHp * healRate));
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
        return $"每次使用调息时，防御提升{defUp * 100:F0}%，最多{maxCount}次（已触发{count}）。";
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
public class HuMaiEffect : StrategyEffect
{
    private const float defUp = 0.10f;

    public HuMaiEffect()
    {
        IsPostiveEffect = true;
        Quality = Quality.Epic;
        Name = "护脉";
    }

    public override string getDiscription()
    {
        return $"每次使用调息时，防御提升{defUp * 100:F0}%。";
    }

    public override void ExcecuteSkill(ref Skill skill)
    {
        base.ExcecuteSkill(ref skill);
        if (skill.Name == "调息")
        {
            Loader.EnhanceDefenseByMultiply(defUp);
        }
    }
}

[System.Serializable]
public class XingMaiEffect : StrategyEffect
{
    private const float atkUp = 0.12f;
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
        return $"每次使用调息时，攻击提升{atkUp * 100:F0}%，最多{maxCount}次（已触发{count}）。";
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
    private const float atkUp = 0.05f;

    public HeHunEffect()
    {
        IsPostiveEffect = true;
        Quality = Quality.Epic;
        Name = "和魂";
    }

    public override string getDiscription()
    {
        return $"每次使用调息时，攻击提升{atkUp * 100:F0}%。";
    }

    public override void ExcecuteSkill(ref Skill skill)
    {
        base.ExcecuteSkill(ref skill);
        if (skill.Name == "调息")
        {
            Loader.EnhanceAttackByMultiply(atkUp);
        }
    }
}
