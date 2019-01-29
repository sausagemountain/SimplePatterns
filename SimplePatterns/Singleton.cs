namespace SimplePatterns
{
    class Singleton
    {
        private static Singleton _instance;
        public static Singleton Instance
        {
            get
            {
                lock (_instance)
                    if (_instance == null)
                        _instance = new Singleton();
                return _instance;
            }
        }

        protected Singleton() { }

        public string OtherProperty { get; set; } = string.Empty;
    }
}