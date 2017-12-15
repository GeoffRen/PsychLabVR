using System;
using System.Linq;

// Enum representing Prompt states. A Prompt contains the prompt and what delay to play the prompt with.
public enum BasicPromptEnum
{
    [BasicPrompt("I’m doing well. I like your new haircut, looks good!", 3f)]
    Prompt0 = 0,
    [BasicPrompt("So I was thinking, maybe we could get together this weekend and hang out some more? I’d like to get to know you better.", 0f)]
    Prompt1 = 1,
    [BasicPrompt("Okay, maybe we can work on the presentation next Tuesday?", 0f)]
    Prompt2 = 2,
    [BasicPrompt("Sure, that sounds good!", 0f)]
    Prompt3 = 3,
    [BasicPrompt("Hey John, can I ask you a quick question?", 4f)]
    Prompt4 = 4,
    [BasicPrompt("I’m wondering whether you can join Mark and me next week to work on the presentation I have next Friday?", 5f)]
    Prompt5 = 5,
    [BasicPrompt("If you have time, I would really appreciate your help.", 0f)]
    Prompt6 = 6,
    END = 7
}

// Extension methods for a Prompt state.
public static class BasicPromptExtensions
{

    // Returns the Prompt.
    public static string GetPrompt(this BasicPromptEnum value)
    {
        return value.GetBasicPromptAttribute<BasicPromptAttribute>().Prompt;
    }

    // Returns the Prompt delay.
    public static float GetDelay(this BasicPromptEnum value)
    {
        return value.GetBasicPromptAttribute<BasicPromptAttribute>().Delay;
    }

    // Generic helper method that returns the Prompt attribute for something.
    public static TAttribute GetBasicPromptAttribute<TAttribute>(this Enum value)
    where TAttribute : Attribute
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        return type.GetField(name)
            .GetCustomAttributes(false)
            .OfType<TAttribute>()
            .SingleOrDefault();
    }

    // Gets the next Prompt state.
    public static BasicPromptEnum next(this BasicPromptEnum value)
    {
        return Enum.GetValues(typeof(BasicPromptEnum)).Cast<BasicPromptEnum>()
                .SkipWhile(e => e != value).Skip(1).First();
    }
}


// A custom attribute containing meta data about the interpretations of a scene.
[AttributeUsage(AttributeTargets.All)]
public class BasicPromptAttribute : Attribute
{
    private string prompt;
    private float delay;

    public BasicPromptAttribute(string prompt, float delay)
    {
        this.prompt = prompt;
        this.delay = delay;
    }

    public string Prompt
    {
        get
        {
            return prompt;
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