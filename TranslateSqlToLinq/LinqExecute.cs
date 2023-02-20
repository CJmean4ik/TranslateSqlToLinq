using System;
using System.Collections.Generic;
using System.Linq;
using TranslateSqlToLinq.Entitys;

namespace TranslateSqlToLinq
{
    class LinqExecute
    {      
        private List<SourseItem> _sourses;
        private readonly Func<List<SourseItem>> _binderSourse;

        public LinqExecute(Func<List<SourseItem>> binderSourse)
        {
            _binderSourse = binderSourse;
        }

        /// <summary>
        /// Метод который обрабатывает колекцию по условию №1:
        /// </summary>
        /// <remarks>
        /// Действия: <br />
        /// Получаетет элементы которые больше 10 и меньше 160 <br />
        /// Сортирует по убыванию <br />
        /// Считает нарастающий итог по полю quantity <br />
        /// Выбирает записи, пока нарастаюищй тог меньше или равен 160 <br />
        /// Выполняет перерасчёт, если нарастающий итог не равен 160 
        /// </remarks>
        /// <returns>колекция NewSourseItem</returns>
        /// <exception cref="NullReferenceException"></exception>
        public List<NewSourseItem> GetAllItemsByFirstCondition()
        {
            if (_binderSourse == null)
                throw new NullReferenceException($"Делегат {_binderSourse.GetType().FullName} не содержит метода для вызова");      
            if (_sourses == null) _sourses = _binderSourse.Invoke();

            int _resultant = 0;
            var listOrderSourse = new List<NewSourseItem>();
            var firstSelection = _sourses
               .Where(w => w.quantity >= 10 && w.quantity <= 160)
               .OrderByDescending(or => or.quantity)                           
               .ToList();
            foreach (var obj in firstSelection)
            {
                int cumulative = CalcCumulativeTotal(_resultant, obj.quantity);
                _resultant = cumulative;
                if (cumulative <= 160)
                {
                    listOrderSourse.Add(new NewSourseItem(obj.Id, obj.item, obj.quantity, cumulative));
                    continue;
                }
                else
                {
                    int result = 160 - cumulative;
                    int newMaxTotal = cumulative + result;
                    listOrderSourse.Add(new NewSourseItem(obj.Id, obj.item, result, newMaxTotal));
                    break;
                }
            }

            return listOrderSourse;
        }

        /// <summary>
        /// Метод который обрабатывает колекцию по условию №2:
        /// </summary>
        /// <remarks>
        /// Действия: <br />
        /// Получаетет элементы которые  меньше 10 <br />
        /// Сортирует по возрастанию <br />
        /// Считает нарастающий итог по полю quantity <br />
        /// Выбирает записи, пока нарастаюищй итог меньше или равен 40 <br />
        /// Выполняет перерасчёт, если нарастающий итог не равен 40 
        /// </remarks>
        /// <returns>колекция NewSourseItem</returns>
        /// <exception cref="NullReferenceException"></exception>
        public List<NewSourseItem> GetAllItemsBySecondCondition()
        {
            if (_binderSourse == null)
                throw new NullReferenceException($"Делегат {_binderSourse.GetType().FullName} не содержит метода для вызова");
          
            if (_sourses == null) _sourses = _binderSourse.Invoke();

            int _resultant = 0;
            var secondSelection = _sourses.Where(w => w.quantity <= 10)
                .OrderBy(or => or.quantity)            
                .ToList();

            var listOrderSourse = new List<NewSourseItem>();

            foreach (var obj in secondSelection)
            {
                int cumulative = CalcCumulativeTotal(_resultant, obj.quantity);
                _resultant = cumulative;
                if (cumulative <= 40)
                {
                    listOrderSourse.Add(new NewSourseItem(obj.Id, obj.item, obj.quantity, cumulative));
                    continue;
                }
                else
                {
                    int result = 40 - cumulative;
                    int newMaxTotal = cumulative + result;
                    listOrderSourse.Add(new NewSourseItem(obj.Id, obj.item, result, newMaxTotal));
                    break;
                }
            }
            return listOrderSourse;
        }

        private int CalcCumulativeTotal(int previousQuantRes, int currentQuant) => previousQuantRes + currentQuant;      
   
    }
    

}
