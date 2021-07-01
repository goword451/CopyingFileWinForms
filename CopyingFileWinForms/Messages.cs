namespace CopyingFileWinForms
{
    public class Messages
    {
        public class Error
        {
            public const string DeserializeError = "Ошибка десериализации";
            public const string DeserializeErrorForLog = "Ошибка десериализации: {0}";
            public const string NotCopiedError = "Копирование не произведено. Отсутствует корневой элемент в файле конфигурации.";
            public const string NotLoadedError = "Файл конфигурации не загружен.";
            public const string NotLoadedErrorForLog = "Файл конфигурации не загружен: {0}";
        }

        public class Success
        {
            public const string SucessfullyDone = "Файл {0} успешно скопирован.";
            public const string SucessfullyCopied = "Копирование завершено.Успешно скопировано {0} из {1} файлов.";
            public const string SucessfullyLoaded = "Файл конфигурации загружен.";
        }

        public class Info
        {
            public const string AbsentSourcePath = "Отсутствует путь источника {0}.";
            public const string AbsentDestinationPath = "Отсутствует путь назначения {0}.";
            public const string AbsentFileName = "Отсутствует файл с именем {0}.";
        }
    }
}
