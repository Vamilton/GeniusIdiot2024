using System;
using System.Collections.Generic;
using System.Linq;

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
                Console.WriteLine("Привет, представься: ");
                string userName = Console.ReadLine();
                do
                {
                    int countRightAnswers = GetRightAnswersCount(countQuestionsAndAnswers, questions, answers);
                    Console.WriteLine("Количество правильных ответов: " + countRightAnswers);
                    Console.WriteLine($"{userName}, твой диагноз: " + GetUserDiagnose(countRightAnswers, countQuestionsAndAnswers, diagnoses));

                }
                while (WantAgain());

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
                diagnoses[1] = "идиот 1";
                diagnoses[2] = "дурак 2";
                diagnoses[3] = "нормальный 3";
                diagnoses[4] = "талант 4";
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
                    {
                        countRightAnswers++;
                    }
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
                    int groupSize = (countQuestionsAndAnswers - 1) / 4;
                    if ((countQuestionsAndAnswers - 1) % 4 == 0)
                    {
                        return diagnoses[(countRightAnswers + groupSize - 1) / groupSize];
                    }
                    else
                    {
                        int groupSizeResidue = (countQuestionsAndAnswers - 1) % 4;
                        if (countRightAnswers <= (groupSize + 1))
                            return diagnoses[1];
                        else // вот тут я начинаю проверять, сколько "групп" входит в заданное количество ответов
                        {
                            int technicalCount = countRightAnswers;
                            int diagnoseCount = 0;
                            int resizeGroup = 1;
                            do
                            {
                                technicalCount -= (groupSize + resizeGroup);
                                if (technicalCount >= 0)
                                    diagnoseCount++;
                                groupSizeResidue--;
                                if (groupSizeResidue <= 0) resizeGroup = 0;
                            } while (technicalCount >= groupSize + resizeGroup * (groupSizeResidue)) ;
                            if (technicalCount > 0)
                                diagnoseCount++;

                            return diagnoses[diagnoseCount];

                        }


                    }
                  
                }

            }

        }
    }
}