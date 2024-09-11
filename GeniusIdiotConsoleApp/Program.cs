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
                int[] columnWidths = new int[] { 25, 28, 10 };
                Console.WriteLine("Привет, представься: ");
                string userName = Console.ReadLine();
                do
                {
                    do
                    {
                        int countRightAnswers = GetRightAnswersCount(countQuestionsAndAnswers, questions, answers);
                        Console.WriteLine("Количество правильных ответов: " + countRightAnswers);
                        string userDiagnose = GetUserDiagnose(countRightAnswers, countQuestionsAndAnswers, diagnoses);
                        Console.WriteLine($"{userName}, твой диагноз: " + userDiagnose);
                        string[] userResults = [userName, countRightAnswers.ToString(), userDiagnose];
                        WriteUserResult(userResults, "results.csv");
                    }

                    while (WantAgain());
                    Console.WriteLine("Показать таблицу результатов? Да/Нет");
                    if (Console.ReadLine().ToLower() == "да")
                        ReadAllResuts("results.csv", columnWidths);
                    else
                        break;
                Console.WriteLine("Показать таблицу результатов? Да/Нет");
                if (Console.ReadLine().ToLower() == "да")
                    ReadAllResuts("results.csv");

                } while (WantAgain());



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

            static int[] GetRandomNumbers(int countQuestionsAndAnswers) // Перемешивание вопросов в рандомном порядке
            {
                Random random = new Random();
                int[] randomNumbers = Enumerable.Range(1, countQuestionsAndAnswers).OrderBy(x => random.Next()).ToArray();
                return randomNumbers;

            }

            static bool WantAgain() // Запрос у пользователя, не хочет ли он повторить
            {
                string answer;
                int attempts = 0;
                do
                {
                    Console.WriteLine("Ещё раз? Да/Нет");
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

            static bool IsNumeric(string unknownAnswer) // Проверка, является ли ответ числом
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


            static void ReadAllResuts(string csvFilePath, int[] columnWidths)
            {
                string[] lines = File.ReadAllLines(csvFilePath, Encoding.UTF8);
                string[][] data = lines.Select(line => line.Split(',')).ToArray();
                for (int i = 0; i < data.Length; i++)
                {
                    for (int j = 0; j < data[i].Length; j++)
                    {
                        string value = data[i][j].Substring(0, Math.Min(data[i][j].Length, columnWidths[j]));
                        value = value.PadRight(columnWidths[j], ' ');

                        Console.Write(value);

                        if (j < data[i].Length - 1)
                            Console.Write(" | ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }
    }
}

