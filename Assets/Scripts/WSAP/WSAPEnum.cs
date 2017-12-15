using System;
using System.Linq;

// Enum representing WSAP states. A WSAP state consists of a sentence, a word, and whether the word is positive or not.
public enum WSAPEnum {
    [WSAP(false, "Uncomfortable", "A friend sets you up on a blind date.")] Trial0 = 0,
    [WSAP(true, "Comfortable", "A friend sets you up on a blind date.")] Trial1 = 1,
    [WSAP(false, "Ugly", "An old friend comments on how you look different now.")] Trial2 = 2,
    [WSAP(true, "Attractive", "An old friend comments on how you look different now.")] Trial3 = 3,
    [WSAP(false, "Mocked", "Everyone stops talking when you enter the room.")] Trial4 = 4,
    [WSAP(true, "Respected", "Everyone stops talking when you enter the room.")] Trial5 = 5,
    [WSAP(false, "Inferior ", "People are confused by your opinions.")] Trial6 = 6,
    [WSAP(true, "Superior ", "People are confused by your opinions.")] Trial7 = 7,
    [WSAP(false, "Embarassed", "People at the wedding laugh at your toast.")] Trial8 = 8,
    [WSAP(true, "Funny", "People at the wedding laugh at your toast.")] Trial9 = 9,
    [WSAP(false, "Dumb", "People judge the speech you just gave.")] Trial10 = 10,
    [WSAP(true, "Intelligent", "People judge the speech you just gave.")] Trial11 = 11,
    [WSAP(false, "Embarassing", "People laugh after something you said.")] Trial12 = 12,
    [WSAP(true, "Funny", "People laugh after something you said.")] Trial13 = 13,
    [WSAP(false, "Unattractive", "People stare at you at a restaurant.")] Trial14 = 14,
    [WSAP(true, "Beautiful", "People stare at you at a restaurant.")] Trial15 = 15,
    [WSAP(false, "Unattractive", "People stare at you while you shop.")] Trial16 = 16,
    [WSAP(true, "Attractive", "People stare at you while you shop.")] Trial17 = 17,
    [WSAP(false, "Hideous", "Someone comments on your new outfit at a party.")] Trial18 = 18,
    [WSAP(true, "Good-Looking", "Someone comments on your new outfit at a party.")] Trial19 = 19,
    [WSAP(false, "Weird", "Someone looks at you as you walk by.")] Trial20 = 20,
    [WSAP(true, "Cool", "Someone looks at you as you walk by.")] Trial21 = 21,
    [WSAP(false, "Clumsy", "Someone you do not know asks you to dance.")] Trial22 = 22,
    [WSAP(true, "Graceful", "Someone you do not know asks you to dance.")] Trial23 = 23,
    [WSAP(false, "Pity", "Someone you like says hello to you.")] Trial24 = 24,
    [WSAP(true, "Admire", "Someone you like says hello to you.")] Trial25 = 25,
    [WSAP(false, "Pissed", "The car behind you is following closely.")] Trial26 = 26,
    [WSAP(true, "Speedy", "The car behind you is following closely.")] Trial27 = 27,
    [WSAP(false, "Trouble", "The cashier calls the manager to help with your purchase")] Trial28 = 28,
    [WSAP(true, "Normal", "The cashier calls the manager to help with your purchase")] Trial29 = 29,
    [WSAP(false, "Angry", "The man in the car next to you looks at you.")] Trial30 = 30,
    [WSAP(true, "Curious", "The man in the car next to you looks at you.")] Trial31 = 31,
    [WSAP(false, "Trapped", "There are a lot of people crowded around the bar.")] Trial32 = 32,
    [WSAP(true, "Popular", "There are a lot of people crowded around the bar.")] Trial33 = 33,
    [WSAP(false, "Embarassing", "You and a classmate accidentally bump into each other.")] Trial34 = 34,
    [WSAP(true, "Funny", "You and a classmate accidentally bump into each other.")] Trial35 = 35,
    [WSAP(false, "Dense", "You are asked to contribute your ideas for a group project.")] Trial36 = 36,
    [WSAP(true, "Clever", "You are asked to contribute your ideas for a group project.")] Trial37 = 37,
    [WSAP(false, "Inadequate", "You are asked to start a new project at work.")] Trial38 = 38,
    [WSAP(true, "Adequate", "You are asked to start a new project at work.")] Trial39 = 39,
    [WSAP(false, "Avoid", "You are invited to a party.")] Trial40 = 40,
    [WSAP(true, "Fun", "You are invited to a party.")] Trial41 = 41,
    [WSAP(false, "Refuse", "You are offered some coffee.")] Trial42 = 42,
    [WSAP(true, "Drink", "You are offered some coffee.")] Trial43 = 43,
    [WSAP(false, "Hideous", "You are on a first date.")] Trial44 = 44,
    [WSAP(true, "Good-Looking", "You are on a first date.")] Trial45 = 45,
    [WSAP(false, "Ugly", "You are playing at the beach.")] Trial46 = 46,
    [WSAP(true, "Attractive", "You are playing at the beach.")] Trial47 = 47,
    [WSAP(false, "Look away", "You are standing next to an attractive person.")] Trial48 = 48,
    [WSAP(true, "Smile", "You are standing next to an attractive person.")] Trial49 = 49,
    [WSAP(false, "Embarassed", "You blush in front of your doctor.")] Trial50 = 50,
    [WSAP(true, "Unnoticed", "You blush in front of your doctor.")] Trial51 = 51,
    [WSAP(false, "Embarassing", "You blush when someone smiles at you.")] Trial52 = 52,
    [WSAP(true, "Cute", "You blush when someone smiles at you.")] Trial53 = 53,
    [WSAP(false, "Stupid", "You do poorly on an exam.")] Trial54 = 54,
    [WSAP(true, "Bad luck", "You do poorly on an exam.")] Trial55 = 55,
    [WSAP(false, "Stupid", "You finish last of everyone on an intelligence test.")] Trial56 = 56,
    [WSAP(true, "Thorough", "You finish last of everyone on an intelligence test.")] Trial57 = 57,
    [WSAP(false, "Disaster", "You have to write an essay about achievements in your life.")] Trial58 = 58,
    [WSAP(true, "Success", "You have to write an essay about achievements in your life.")] Trial59 = 59,
    [WSAP(false, "Rejected", "You hear a friend make a joke about you.")] Trial60 = 60,
    [WSAP(true, "Accepted", "You hear a friend make a joke about you.")] Trial61 = 61,
    [WSAP(false, "Mocked", "You hear your name mentioned in a nearby conversation.")] Trial62 = 62,
    [WSAP(true, "Respected", "You hear your name mentioned in a nearby conversation.")] Trial63 = 63,
    [WSAP(false, "Stupid", "You just finished taking an oral exam.")] Trial64 = 64,
    [WSAP(true, "Smart", "You just finished taking an oral exam.")] Trial65 = 65,
    [WSAP(false, "Ugly", "You just got your yearbook pictures back.")] Trial66 = 66,
    [WSAP(true, "Attractive", "You just got your yearbook pictures back.")] Trial67 = 67,
    [WSAP(false, "Awkward", "You make small talk with people at the wedding reception.")] Trial68 = 68,
    [WSAP(true, "Polite", "You make small talk with people at the wedding reception.")] Trial69 = 69,
    [WSAP(false, "Helpless", "You need help with a report.")] Trial70 = 70,
    [WSAP(true, "Capable", "You need help with a report.")] Trial71 = 71,
    [WSAP(false, "Undesirable", "You notice a group of your peers watching you sing.")] Trial72 = 72,
    [WSAP(true, "Desirable", "You notice a group of your peers watching you sing.")] Trial73 = 73,
    [WSAP(false, "Rejection", "You receive a call from a company you interviewed with.")] Trial74 = 74,
    [WSAP(true, "Acceptance", "You receive a call from a company you interviewed with.")] Trial75 = 75,
    [WSAP(false, "Dumb", "You receive an unexpected grade on your test.")] Trial76 = 76,
    [WSAP(true, "Intelligent", "You receive an unexpected grade on your test.")] Trial77 = 77,
    [WSAP(false, "Walk away", "You see a group of people approaching.")] Trial78 = 78,
    [WSAP(true, "Greet", "You see a group of people approaching.")] Trial79 = 79,
    [WSAP(false, "Loser", "You see an attractive person looking at you in the store.")] Trial80 = 80,
    [WSAP(true, "Loved", "You see an attractive person looking at you in the store.")] Trial81 = 81,
    [WSAP(false, "Dense", "You share an idea with someone.")] Trial82 = 82,
    [WSAP(true, "Clever", "You share an idea with someone.")] Trial83 = 83,
    [WSAP(false, "Uncomfortable", "You stand up to introduce yourself at a meeting.")] Trial84 = 84,
    [WSAP(true, "Comfortable", "You stand up to introduce yourself at a meeting.")] Trial85 = 85,
    [WSAP(false, "Confused", "You take a long time to make decisions about the future.")] Trial86 = 86,
    [WSAP(true, "Careful", "You take a long time to make decisions about the future.")] Trial87 = 87,
    [WSAP(false, "Criticize", "Your boss wants to meet with you.")] Trial88 = 88,
    [WSAP(true, "Praise", "Your boss wants to meet with you.")] Trial89 = 89,
    [WSAP(false, "Scary", "Your career advisor wants to meet with you.")] Trial90 = 90,
    [WSAP(true, "Helpful", "Your career advisor wants to meet with you.")] Trial91 = 91,
    [WSAP(false, "Disaster", "Your Christmas party turns out different than last year.")] Trial92 = 92,
    [WSAP(true, "Better", "Your Christmas party turns out different than last year.")] Trial93 = 93,
    [WSAP(false, "Gossip", "Your coworkers stop talking when you enter the room.")] Trial94 = 94,
    [WSAP(true, "Greeting", "Your coworkers stop talking when you enter the room.")] Trial95 = 95,
    [WSAP(false, "Stay home", "Your friend asks you to go to a party.")] Trial96 = 96,
    [WSAP(true, "Dance", "Your friend asks you to go to a party.")] Trial97 = 97,
    [WSAP(false, "Pity", "Your friend comments on your new haircut.")] Trial98 = 98,
    [WSAP(true, "Admire", "Your friend comments on your new haircut.")] Trial99 = 99,
    [WSAP(false, "Upset", "Your friend does not call you back.")] Trial100 = 100,
    [WSAP(true, "Try later", "Your friend does not call you back.")] Trial101 = 101,
    [WSAP(false, "Panicky", "Your picture is going to be in the newspaper.")] Trial102 = 102,
    [WSAP(true, "Excited", "Your picture is going to be in the newspaper.")] Trial103 = 103,
    [WSAP(false, "Uncomfortable", "Your teacher calls on you to answer.")] Trial104 = 104,
    [WSAP(true, "Capable", "Your teacher calls on you to answer.")] Trial105 = 105,
    [WSAP(false, "Unqualified", "You get a new job.")] Trial106 = 106,
    [WSAP(true, "Qualified", "You get a new job.")] Trial107 = 107,
    [WSAP(false, "Inadequate", "You have been asked to take on a new responsibility at work.")] Trial108 = 108,
    [WSAP(true, "Adequate", "You have been asked to take on a new responsibility at work.")] Trial109 = 109,
    [WSAP(false, "Worthless", "Your boss ignores your input.")] Trial110 = 110,
    [WSAP(true, "Distracted", "Your boss ignores your input.")] Trial111 = 111,
    [WSAP(false, "Defective", "People always tell you to smile.")] Trial112 = 112,
    [WSAP(true, "Loved", "People always tell you to smile.")] Trial113 = 113,
    [WSAP(false, "Defective", "People tell you to laugh more often.")] Trial114 = 114,
    [WSAP(true, "Loved", "People tell you to laugh more often.")] Trial115 = 115,
    [WSAP(false, "Unwanted", "You are told by your parents about your sister getting married.")] Trial116 = 116,
    [WSAP(true, "Wanted", "You are told by your parents about your sister getting married.")] Trial117 = 117,
    [WSAP(false, "Distressed", "You lie awake in bed.")] Trial118 = 118,
    [WSAP(true, "Excited", "You lie awake in bed.")] Trial119 = 119,
    END = 120
}

// Extension methods for a WSAP state.
public static class WSAPExtensions
{

    // Returns whether the word is positive or not.
    public static bool GetPositive(this WSAPEnum value)
    {
        return value.GetAttribute<WSAPAttribute>().Positive;
    }

    // Returns meta data for the word.
    public static string GetWord(this WSAPEnum value)
    {
        return value.GetAttribute<WSAPAttribute>().Word;
    }

    // Returns meta data for the sentence.
    public static string GetSentence(this WSAPEnum value)
    {
        return value.GetAttribute<WSAPAttribute>().Sentence;
    }

    // Generic helper method that returns the attribute associated with something.
    private static TAttribute GetAttribute<TAttribute>(this Enum value)
    where TAttribute : Attribute
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        return type.GetField(name) 
            .GetCustomAttributes(false)
            .OfType<TAttribute>()
            .SingleOrDefault();
    }

    // Gets the next WSAP state.
    public static WSAPEnum next(this WSAPEnum value)
    {
        return Enum.GetValues(typeof(WSAPEnum)).Cast<WSAPEnum>()
                .SkipWhile(e => e != value).Skip(1).First();
    }
}


// A custom attribute WSAPAttribute containing meta data about the words associated with a sentence.
[AttributeUsage(AttributeTargets.All)]
public class WSAPAttribute : Attribute
{
    private bool positive;
    private string word;
    private string sentence;

    public WSAPAttribute(bool positive, string word, string sentence)
    {
        this.positive = positive;
        this.word = word;
        this.sentence = sentence;
    }

    public bool Positive
    {
        get
        {
            return positive;
        }
    }

    public string Word
    {
        get
        {
            return word;
        }
    }

    public string Sentence
    {
        get
        {
            return sentence;
        }
    }
}