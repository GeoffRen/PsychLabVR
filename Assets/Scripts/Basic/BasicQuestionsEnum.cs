using System;
using System.Linq;

// Enum representing Question states. A Question contains a question, what delay to play the question with, and a positive, neutral, or negative interpretation fo the question.
public enum BasicQuestionsEnum
{
    [BasicQuestions("He is surprised and flattered", "He didn't expect you to compliment him", "He thought it was weird you gave a compliment", "How is Mark feeling?", 4f)]
    Question0 = 0,
    [BasicQuestions("Although he wants to hang out with you, he can’t this weekend", "He’s busy this weekend", "He doesn’t want to hang out with you outside of work", "Why did Mark say this?", 9f)]
    Question1 = 1,
    [BasicQuestions("He wants to make sure you do your best on the presentation", "He wants John to help too", "He feels you’re too awkward to hang out with one on one", "Why did Mark say that? ", 8f)]
    Question2 = 2,
    [BasicQuestions("He’s interested in talking but just needs to finish up his current task", "He’s busy and will respond momentarily", "He’s annoyed that you’re bothering him", "What is John feeling?", 5.5f)]
    Question3 = 3,
    [BasicQuestions("John would help, but he has a lot of work", "John is busy", "John doesn't want to help me", "Why did John respond the way he did?", 6f)]
    Question4 = 4,
    [BasicQuestions("John wants to make sure your project goes well", "John understands you could use the help", "John thinks you're incompetent", "Why did John say yes?", 6f)]
    Question5 = 5,
    END = 6
}

// Extension methods for a Question state.
public static class BasicQuestionsExtensions
{

    // Returns the positive interpretation.
    public static string GetPositive(this BasicQuestionsEnum value)
    {
        return value.GetBasicQuestionAttribute<BasicQuestionsAttribute>().Positive;
    }

    // Returns the neutral interpretation.
    public static string GetNeutral(this BasicQuestionsEnum value)
    {
        return value.GetBasicQuestionAttribute<BasicQuestionsAttribute>().Neutral;
    }

    // Returns the negative interpretation.
    public static string GetNegative(this BasicQuestionsEnum value)
    {
        return value.GetBasicQuestionAttribute<BasicQuestionsAttribute>().Negative;
    }

    // Returns the question's sentence.
    public static string GetSentence(this BasicQuestionsEnum value)
    {
        return value.GetBasicQuestionAttribute<BasicQuestionsAttribute>().Question;
    }

    // Returns what delay to play the question with.
    public static float GetDelay(this BasicQuestionsEnum value)
    {
        return value.GetBasicQuestionAttribute<BasicQuestionsAttribute>().Delay;
    }

    // Generic helper method that returns the Question attribute for something.
    public static TAttribute GetBasicQuestionAttribute<TAttribute>(this Enum value)
    where TAttribute : Attribute
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        return type.GetField(name)
            .GetCustomAttributes(false)
            .OfType<TAttribute>()
            .SingleOrDefault();
    }

    // Gets the next Question state.
    public static BasicQuestionsEnum next(this BasicQuestionsEnum value)
    {
        return Enum.GetValues(typeof(BasicQuestionsEnum)).Cast<BasicQuestionsEnum>()
                .SkipWhile(e => e != value).Skip(1).First();
    }
}


// A custom attribute containing meta data about the interpretations of a scene.
[AttributeUsage(AttributeTargets.All)]
public class BasicQuestionsAttribute : Attribute
{
    private string positive;
    private string neutral;
    private string negative;
    private string question;
    private float delay;

    public BasicQuestionsAttribute(string positive, string neutral, string negative, string question, float delay)
    {
        this.positive = positive;
        this.neutral = neutral;
        this.negative = negative;
        this.question = question;
        this.delay = delay;
    }

    public string Positive
    {
        get
        {
            return positive;
        }
    }

    public string Neutral
    {
        get
        {
            return neutral;
        }
    }

    public string Negative
    {
        get
        {
            return negative;
        }
    }

    public string Question
    {
        get
        {
            return question;
        }
    }

    public float Delay
    {
        get
        {
            return delay;
        }
    }
}