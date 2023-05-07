using TdLib;

namespace egartbot.Modules
{
    public class FixKeyboardLayout : ModuleBase
    {
        public FixKeyboardLayout()
        {
            Subscribe<TdApi.Update.UpdateNewMessage>(Process, this);
        }

        public async Task Process(TdApi.Update.UpdateNewMessage updateNewMessage)
        {
            if (updateNewMessage.Message.IsOutgoing && updateNewMessage.Message.Content is TdApi.MessageContent.MessageText messageText && updateNewMessage.Message.ReplyToMessageId != 0)
            {
                var chat = await ExecuteAsync(new TdApi.GetChat
                {
                    ChatId = updateNewMessage.Message.ChatId
                });

                var text = messageText.Text.Text;

                if (text == "!")
                {
                    await ExecuteAsync(new TdApi.DeleteMessages
                    {
                        ChatId = chat.Id,
                        MessageIds = new long[] { updateNewMessage.Message.Id },
                        Revoke = true
                    });

                    var messageToFix = await ExecuteAsync(new TdApi.GetMessage 
                    {
                        ChatId = chat.Id,
                        MessageId = updateNewMessage.Message.ReplyToMessageId
                    });

                    if (messageToFix.Content is TdApi.MessageContent.MessageText messageToFixText)
                    {
                        var textToFix = messageToFixText.Text.Text;

                        var fixedText = "";

                        foreach (var c in textToFix)
                        {
                            fixedText += layout[c];
                        }

                        await ExecuteAsync(new TdApi.EditMessageText
                        {
                            ChatId = chat.Id,
                            MessageId = messageToFix.Id,
                            InputMessageContent = new TdApi.InputMessageContent.InputMessageText
                            {
                                Text = new TdApi.FormattedText
                                {
                                    Text = fixedText
                                }
                            }
                        });
                    }
                }
            }
        }
        public readonly Dictionary<char, char> layout = new()
        {
            ['`'] = 'ё',
            ['q'] = 'й',
            ['w'] = 'ц',
            ['e'] = 'у',
            ['r'] = 'к',
            ['t'] = 'е',
            ['y'] = 'н',
            ['u'] = 'г',
            ['i'] = 'ш',
            ['o'] = 'щ',
            ['p'] = 'з',
            ['['] = 'х',
            [']'] = 'ъ',
            ['a'] = 'ф',
            ['s'] = 'ы',
            ['d'] = 'в',
            ['f'] = 'а',
            ['g'] = 'п',
            ['h'] = 'р',
            ['j'] = 'о',
            ['k'] = 'л',
            ['l'] = 'д',
            [';'] = 'ж',
            ['\''] = 'э',
            ['z'] = 'я',
            ['x'] = 'ч',
            ['c'] = 'с',
            ['v'] = 'м',
            ['b'] = 'и',
            ['n'] = 'т',
            ['m'] = 'ь',
            [','] = 'б',
            ['.'] = 'ю',
            ['/'] = '.',
            ['~'] = 'Ё',
            ['Q'] = 'Й',
            ['W'] = 'Ц',
            ['E'] = 'У',
            ['R'] = 'К',
            ['T'] = 'Е',
            ['Y'] = 'Н',
            ['U'] = 'Г',
            ['I'] = 'Ш',
            ['O'] = 'Щ',
            ['P'] = 'З',
            ['{'] = 'Х',
            ['}'] = 'Ъ',
            ['A'] = 'Ф',
            ['S'] = 'Ы',
            ['D'] = 'В',
            ['F'] = 'А',
            ['G'] = 'П',
            ['H'] = 'Р',
            ['J'] = 'О',
            ['K'] = 'Л',
            ['L'] = 'Д',
            [':'] = 'Ж',
            ['"'] = 'Э',
            ['Z'] = 'Я',
            ['X'] = 'Ч',
            ['C'] = 'С',
            ['V'] = 'М',
            ['B'] = 'И',
            ['N'] = 'Т',
            ['M'] = 'Ь',
            ['<'] = 'Б',
            ['>'] = 'Ю',
            ['?'] = ',',
            ['@'] = '"',
            ['#'] = '№',
            ['$'] = ';',
            ['^'] = ':',
            ['&'] = '?',
            [' '] = ' ',
            ['!'] = '!',
            ['%'] = '%',
            ['*'] = '*',
            ['('] = '(',
            [')'] = ')',
            ['-'] = '-',
            ['_'] = '_',
            ['='] = '=',
            ['+'] = '+',
        };
    }
}