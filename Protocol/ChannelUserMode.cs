namespace ReactiveIRC.Protocol
{
    public static class ChannelUserMode
    {
        public static char SymbolToMode(char symbol)
        {
            switch(symbol)
            {
                case '+': return 'v';
                case '%': return 'h';
                case '@': return 'o';
                case '&': return 'p';
                case '~': return 'q';
            }
            return char.MinValue;
        }

        public static char ModeToSymbol(char symbol)
        {
            switch(symbol)
            {
                case 'v': return '+';
                case 'h': return '%';
                case 'o': return '@';
                case 'p': return '&';
                case 'q': return '~';
            }
            return char.MinValue;
        }
    }
}
