using System;
using System.Collections.Generic;
using System.Linq;
using TranslateSqlToLinq.Entitys;

namespace TranslateSqlToLinq
{
    class LinqExecute
    {
        private int _resultant;
        private List<SourseItem> _sourses;
        private readonly Func<List<SourseItem>> _binderSourse;

        public LinqExecute(Func<List<SourseItem>> binderSourse)
        {
            _binderSourse = binderSourse;
        }

        /// <summary>
        /// Метод который обрабатывает колекцию по условию №1:
        /// </summary>
        /// <returns>List<NewSourseItem> который содержит столбец total для нарастающего итога</returns>
        /// <exception cref="NullReferenceException"></exception>
        public List<NewSourseItem> GetAllItemsByFirstCondition()
        {
            if (_binderSourse == null)
                throw new NullReferenceException($"Делегат {_binderSourse.GetType().FullName} не содержит метода для вызова");      
            if (_sourses == null) _sourses = _binderSourse.Invoke();

            _resultant = 0;
            var firstSelection = _sourses
               .Where(w => w.quantity >= 10 && w.quantity <= 160)
               .OrderByDescending(or => or.quantity)
               .Select(s =>
               {

                   int cumulative = CalcCumulativeTotal(_resultant, s.quantity);
                   _resultant = cumulative;
                   return new { s.Id, s.item, s.quantity, cumulative };
               })
               .Where(w => w.cumulative <= 160)
               .ToList();

            int maxTotal = firstSelection.Max(mx => mx.cumulative);
            int maxTotalId = firstSelection.Where(w => w.cumulative == maxTotal).Select(s => s.Id).FirstOrDefault();

            if (maxTotal != 160)
            {
                var newElemen = FindingDifference(maxTotal, maxTotalId, TypeCondition.FirstCondition);
                firstSelection.Add(new { newElemen.Id, newElemen.item, newElemen.quantity, newElemen.cumulative });
            }
            return firstSelection.Select(s => new NewSourseItem(s.Id, s.item,s.quantity,s.cumulative)).ToList();
        }        
        public List<NewSourseItem> GetAllItemsBySecondCondition()
        {
            if (_binderSourse == null)
                throw new NullReferenceException($"Делегат {_binderSourse.GetType().FullName} не содержит метода для вызова");
          
            if (_sourses == null) _sourses = _binderSourse.Invoke();

            _resultant = 0;
            var secondSelection = _sourses.Where(w => w.quantity <= 10)
                .OrderBy(or => or.quantity)
                .Select(s => 
                {
                    int cumulative = CalcCumulativeTotal(_resultant, s.quantity);
                    _resultant = cumulative;
                    return new { s.Id, s.item, s.quantity, cumulative };
                })
                .Where(w => w.cumulative <= 40) 
                .ToList();
           
            int maxTotal = secondSelection.Max(mx => mx.cumulative);
            int maxTotalId = secondSelection.Where(w => w.cumulative == maxTotal).Select(s => s.Id).FirstOrDefault();
            if (maxTotal != 40)
            {
                var newElemen = FindingDifference(maxTotal, maxTotalId, TypeCondition.SecondCondition);
                secondSelection.Add(new { newElemen.Id, newElemen.item, newElemen.quantity, newElemen.cumulative });
            }
            return secondSelection.Select(s => new NewSourseItem(s.Id, s.item, s.quantity, s.cumulative)).ToList();
        }

        private int CalcCumulativeTotal(int previousQuantRes, int currentQuant) => previousQuantRes + currentQuant;      
        private (int Id, string item, int quantity, int cumulative) FindingDifference(int maxTotal, int id,TypeCondition condition)
        {
            if (condition == TypeCondition.FirstCondition)
            {             
                    int result = 160 - maxTotal;
                    int newMaxTotal = maxTotal + result;
                    if (newMaxTotal == 160)
                        return (id + 1, _sourses[id].item, result, newMaxTotal);               
            }
            if (condition == TypeCondition.SecondCondition)
            {              
                    int result = 40 - maxTotal;
                    int newMaxTotal = maxTotal + result;
                    if (newMaxTotal == 40)
                        return (id + 1, _sourses[id].item, result, newMaxTotal);              
            }
            return default;
        }
    }
    enum TypeCondition
    {
        FirstCondition,
        SecondCondition
    }

}
