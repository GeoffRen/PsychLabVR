using System;
using System.Linq;

// Enum representing SPIN states. A SPIN state consists of a question.
public enum SPINEnum
{
    [SPIN("I am afraid of people in authority.")] Question0 = 0,
    [SPIN("I am bothered by blushing in front of people.")] Question1 = 1,
    [SPIN("Parties and social events scare me.")] Question2 = 2,
    [SPIN("I avoid talking to people I don’t know.")] Question3 = 3,
    [SPIN("Being criticized scares me a lot")] Question4 = 4,
    [SPIN("I avoid doing things or speaking to people for fear of embarrassment.")] Question5 = 5,
    [SPIN("Sweating in front of people causes me distress.")] Question6 = 6,
    [SPIN("I avoid going to parties.")] Question7 = 7,
    [SPIN("I avoid activities in which I am the center of attention.")] Question8 = 8,
    [SPIN("Talking to strangers scares me.")] Question9 = 9,
    [SPIN("I avoid having to give speeches.")] Question10 = 10,
    [SPIN("I would do anything to avoid being criticized.")] Question11 = 11,
    [SPIN("Heart palpitations bother me when I am around people.")] Question12 = 12,
    [SPIN("I am afraid of doing things when people might be watching.")] Question13 = 13,
    [SPIN("Being embarrassed or looking stupid are among my worst fears.")] Question14 = 14,
    [SPIN("I avoid speaking to anyone in authority.")] Question15 = 15,
    [SPIN("Trembling or shaking in front of others is distressing to me.")] Question16 = 16,
    END = 17
}

// Extension methods for a SPIN state.
public static class SPINExtensions
{

    // Returns the SPIN question.
    public static string GetQuestion(this SPINEnum value)
    {
        return value.GetSPINAttribute<SPINAttribute>().Question;
    }

    // Generic helper method that returns the SPIN attribute for something.
    public static TAttribute GetSPINAttribute<TAttribute>(this Enum value)
    where TAttribute : Attribute
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        return type.GetField(name)
            .GetCustomAttributes(false)
            .OfType<TAttribute>()
            .SingleOrDefault();
    }

    // Gets the next SPIN state.
    public static SPINEnum next(this SPINEnum value)
    {
        return Enum.GetValues(typeof(SPINEnum)).Cast<SPINEnum>()
                .SkipWhile(e => e != value).Skip(1).First();
    }
}


// A custom attribute containing meta data about the interpretations of a scene.
[AttributeUsage(AttributeTargets.All)]
public class SPINAttribute : Attribute
{
    private string question;

    public SPINAttribute(string question)
    {
        this.question = question;
    }

    public string Question
    {
        get
        {
            return question;
        }
    }
}