using System;

[Serializable]
public class Identity 
{   
    public Sex sex;
    public bool canReproduce;

    public Identity(Sex sex, bool isBaby)
    {
        this.sex = sex;
        if(isBaby)
            canReproduce = false;
        else
            canReproduce = true;
        
    }
}

public enum Sex { MALE, FEMALE }
