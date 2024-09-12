using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Text;

namespace GeniusIdiotConsoleApp
{
    internal class Program
    {

        class Programm
        {

            static void Main(string[] args)
            {
                int countQuestionsAndAnswers = 9;
                string[] questions = GetQuestions(countQuestionsAndAnswers);
                int[] answers = GetAnswers(countQuestionsAndAnswers);
                string[] diagnoses = GetDiagnoses();
                string csvFilePath = "results.csv";
                string[] questionsToRepeat = ["Ещё раз?", "Показать таблицу результатов?"];
                Console.WriteLine("Привет, представься: ");
                string userName = Console.ReadLine();
                do
                {
                    int countRightAnswers = GetRightAnswersCount(countQuestionsAndAnswers, questions, answers);
                    Console.WriteLine("Количество правильных ответов: " + countRightAnswers);
                    string userDiagnose = GetUserDiagnose(countRightAnswers, countQuestionsAndAnswers, diagnoses);
                    Console.WriteLine($"{userName}, твой диагноз: " + userDiagnose);
                    string[] userResults = [userName, countRightAnswers.ToString(), userDiagnose];
                    WriteUserResult(userResults, csvFilePath);
                    if (WantAgain(questionsToRepeat[1]))
                        ReadAllResuts(csvFilePath);
                } while (WantAgain(questionsToRepeat[0]));

            }

            static string[] GetQuestions(int countQuestionsAndAnswers)
            {
                string[] questions = new string[countQuestionsAndAnswers];
                questions[0] = "Сколько будет два плюс два умноженное на два?";
                questions[1] = "Бревно нужно распилить на 10 частей. Сколько распилов нужно сделать?";
                questions[2] = "На двух руках 10 пальцев. Сколько пальцев на 5 руках?";
                questions[3] = "Укол делают каждые полчаса. Сколько нужно минут, чтобы сделать три укола?";
                questions[4] = "Пять свечей горело, две потухли. Сколько свечей осталось?";
                questions[5] = "Какой сейчас год?";
                questions[6] = "Напиши 44";
                questions[7] = "Сколько в рубле копеек?";
                questions[8] = "Сколько дней недели?";
                return questions;
            }

            static int[] GetAnswers(int countQuestionsAndAnswers)
            {
                int[] answers = new int[countQuestionsAndAnswers];
                answers[0] = 6;
                answers[1] = 9;
                answers[2] = 25;
                answers[3] = 60;
                answers[4] = 2;
                answers[5] = 2024;
                answers[6] = 44;
                answers[7] = 100;
                answers[8] = 7;
                return answers;
            }

            static string[] GetDiagnoses()
            {
                string[] diagnoses = new string[6];
                diagnoses[0] = "кретин";
                diagnoses[1] = "идиот";
                diagnoses[2] = "дурак";
                diagnoses[3] = "нормальный";
                diagnoses[4] = "талант";
                diagnoses[5] = "гений";
                return diagnoses;
            }

            private static int[] GetRandomNumbers(int countQuestionsAndAnswers) // Перемешивание вопросов в рандомном порядке
            {
                Random random = new Random();
                int[] randomNumbers = Enumerable.Range(1, countQuestionsAndAnswers).OrderBy(x => random.Next()).ToArray();
                return randomNumbers;

            }

            private static bool WantAgain(string question) // Запрос у пользователя, не хочет ли он повторить
            {
                string answer;
                int attempts = 0;
                do
                {
                    Console.WriteLine($"{question} Да/Нет");
                    answer = Console.ReadLine().ToLower();
                    if (answer == "да") return true;
                    else if (answer == "нет") return false;
                    else
                    {
                        Console.Write(attempts == 2 ? "Будем считать, что нет." : "Непонятно. ");
                        attempts++;
                    }

                } while (answer != "да" && answer != "нет" && attempts < 3);
                return false;
            }

            static int GetRightAnswersCount(int countQuestionsAndAnswers, string[] questions, int[] answers) // Основной опросник
            {
                int countRightAnswers = 0;
                int[] randomNumbers = GetRandomNumbers(countQuestionsAndAnswers);
                foreach (int number in randomNumbers)
                {
                    Console.WriteLine("Вопрос №" + (number));
                    Console.WriteLine(questions[number - 1]);

                    int userAnswer;
                    string unknownAnswer = Console.ReadLine();
                    while (!IsNumeric(unknownAnswer))
                    {
                        Console.WriteLine("В качестве ответа должно быть число.");
                        unknownAnswer = Console.ReadLine();
                    }
                    userAnswer = int.Parse(unknownAnswer);
                    int rightAnswer = answers[number - 1];

                    if (userAnswer == rightAnswer)
                        countRightAnswers++;
                }
                return countRightAnswers;
            }

            private static bool IsNumeric(string unknownAnswer) // Проверка, является ли ответ числом
            {
                int number;
                return int.TryParse(unknownAnswer, out number);
            }

            static string GetUserDiagnose(int countRightAnswers, int countQuestionsAndAnswers, string[] diagnoses)
            {
                if (countRightAnswers == 0)
                    return diagnoses[0];
                else if (countRightAnswers == countQuestionsAndAnswers)
                    return diagnoses[5];
                else
                {
                    double percentRightAnswers = ((double)countRightAnswers / countQuestionsAndAnswers) * 100;
                    if (percentRightAnswers < 25)
                        return diagnoses[1];
                    else
                    {
                        if (percentRightAnswers < 50)
                            return diagnoses[2];
                        else
                        {
                            if (percentRightAnswers < 75)
                                return diagnoses[3];
                            else
                                return diagnoses[4];
                        }
                    }
                }
            }


            static void WriteUserResult(string[] userResults, string filePath)
            {
                using (StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8))
                {
                    writer.WriteLine(string.Join(",", userResults));
                }
            }


            static void ReadAllResuts(string csvFilePath)
            {
                int[] columnWidths = new int[] { GetMaxFirstColumnWidth(csvFilePath), 28, 10 };
                var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ",", HasHeaderRecord = false };
                config.MissingFieldFound = null;
                using (var reader = new StreamReader(csvFilePath))
                using (var csv = new CsvReader(reader, config))
                {
                    var records = csv.GetRecords<ResultsTable>().ToList();
                    foreach (var record in records)
                    {
                        Console.WriteLine("{0,-" + columnWidths[0] + "}{1,-" + columnWidths[1] + "}{2,-" + columnWidths[2] + "}",
                                          record.Name+ " ",
                                          record.countRightAnswers.ToString(),
                                          record.diagnose);
                    }
                }
            }

            private class ResultsTable
            {
                public string Name { get; set; }
                public string countRightAnswers { get; set; }
                public string diagnose { get; set; }
            }


            private static int GetMaxFirstColumnWidth(string csvFilePath)
            {
                using (var reader = new StreamReader(csvFilePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    int maxFirstColumnWidth = 0;
                    while (csv.Read())
                    {
                        string firstColumnValue = csv.GetField<string>(0);
                        if (firstColumnValue.Length > maxFirstColumnWidth)
                            maxFirstColumnWidth = firstColumnValue.Length;
                    }
                    return maxFirstColumnWidth+1;
                }
            }

        }
    }
}

