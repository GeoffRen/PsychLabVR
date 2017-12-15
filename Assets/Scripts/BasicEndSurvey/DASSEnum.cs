using System;
using System.Linq;

// Enum representing DASS states. A DASS state consists of a question.
public enum DASSEnum
{
    [DASS("I found myself getting upset by quite trivial things.")]
    Question0 = 0,
    [DASS("I was aware of dryness of my mouth.")]
    Question1 = 1,
    [DASS("I couldn’t seem to experience any positive feeling at all.")]
    Question2 = 2,
    [DASS("I experienced breathing difficulty (e.g., excessively rapid breathing, breathlessness in the absence of physical exertion).")]
    Question3 = 3,
    [DASS("I just couldn’t seem to get going.")]
    Question4 = 4,
    [DASS("I tended to over-react to situations.")]
    Question5 = 5,
    [DASS("I had a feeling of shakiness (e.g., legs going to give way).")]
    Question6 = 6,
    [DASS("I found it difficult to relax.")]
    Question7 = 7,
    [DASS("I found myself in situations that made me so anxious I was most relieved when they ended.")]
    Question8 = 8,
    [DASS("I felt that I had nothing to look forward to.")]
    Question9 = 9,
    [DASS("II found myself getting upset rather easily.")]
    Question10 = 10,
    [DASS("I felt that I was using a lot of nervous energy.")]
    Question11 = 11,
    [DASS("I felt sad and depressed.")]
    Question12 = 12,
    [DASS("I found myself getting impatient when I was delayed in any way (e.g., elevators, traffic lights, being kept waiting).")]
    Question13 = 13,
    [DASS("I had a feeling of faintness.")]
    Question14 = 14,
    [DASS("I felt that I had lost interest in just about everything.")]
    Question15 = 15,
    [DASS("I felt I wasn’t worth much as a person.")]
    Question16 = 16,
    [DASS("I felt that I was rather touchy.")]
    Question17 = 17,
    [DASS("I perspired noticeably (e.g., hands sweaty) in the absence of high temperatures or physical exertion.")]
    Question18 = 18,
    [DASS("I felt scared without any good reason.")]
    Question19 = 19,
    [DASS("I felt that life wasn’t worthwhile. ")]
    Question20 = 20,
    END = 21
}

// Extension methods for a DASS state.
public static class DASSExtensions
{

    // Returns the DASS question.
    public static string GetQuestion(this DASSEnum value)
    {
        return value.GetDASSAttribute<DASSAttribute>().Question;
    }

    // Generic helper method that returns the DASS attribute for something.
    public static TAttribute GetDASSAttribute<TAttribute>(this Enum value)
    where TAttribute : Attribute
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        return type.GetField(name)
            .GetCustomAttributes(false)
            .OfType<TAttribute>()
            .SingleOrDefault();
    }

    // Gets the next DASS state.
    public static DASSEnum next(this DASSEnum value)
    {
        return Enum.GetValues(typeof(DASSEnum)).Cast<DASSEnum>()
                .SkipWhile(e => e != value).Skip(1).First();
    }
}


// A custom attribute containing meta data about the interpretations of a scene.
[AttributeUsage(AttributeTargets.All)]
public class DASSAttribute : Attribute
{
    private string question;

    public DASSAttribute(string question)
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