using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.ViewModels
{
    public class ArrayServices
    {
        public decimal[] combineArrays(decimal[] first, decimal[] second)
        {
            decimal[] values = new decimal[12];
            if (first == null)
            {
                values = second;
            }
            else if (second == null)
            {
                values = first;
            }
            else
            {
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = first[i] + second[i];
                }
            }


            return values;
        }

        public decimal[] cumulativeArray(decimal[] array)
        {
            decimal[] values = new decimal[12];

            for (var i = 0; i < 12; i++)
            {
                if(i == 0)
                {
                    values[i] = array[i];
                }else
                {
                    values[i] = array[i] + values[i - 1];
                }
            }


            return values;
        }

        public decimal[] valueToArray(decimal value)
        {
            decimal[] data = new decimal[12];

            for(var i = 0; i < 12; i++)
            {
                data[i] = value / 12;
            }

            return data;
        }

        public decimal[] combineArrays(decimal[] first, decimal[] second, decimal[] third)
        {
            decimal[] values = new decimal[12];
            values = combineArrays(first, second);
            values = combineArrays(values, third);

            return values;
        }

        public decimal[] combineArrays(decimal[] first, decimal[] second, decimal[] third, decimal[] four)
        {
            decimal[] values = new decimal[12];
            values = combineArrays(first, second, third);
            values = combineArrays(values, four);

            return values;
        }


        public decimal[] multiplyArraybyValue(decimal[] data, decimal value)
        {
            decimal[] values = new decimal[12];
            for (var i = 0; i < values.Length; i++)
            {
                if (value != 0)
                {
                    values[i] = data[i] * value;
                }
            }

            return data;
        }

        public decimal[] multiplyArrays(decimal[] first, decimal[] second)
        {
            decimal[] values = new decimal[12];
            for (var i = 0; i < 12; i++)
            {
                 values[i] = first[i] * second[i];
            }

            return values;
        }

        public decimal[] subtractArrays(decimal[] array, decimal[] other)
        {
            decimal[] values = new decimal[12];

            for (var i = 0; i < 12; i++)
            {
                values[i] = array[i] - other[i];
            }

            return values;
        }

        public decimal[] sortData(List<QueryableData> data)
        {
            decimal[] sortedData = new decimal[12];

            if (data.FirstOrDefault() != null)
            {
                CounsellingServicesQueries q = new CounsellingServicesQueries(data.FirstOrDefault().Year);

                for (var i = 0; i < 12; i++)
                {
                    sortedData[i] = q.retreiveMonthlyValue(data, i + 1);
                }


            }

            return sortedData;
        }

        public decimal sumArray(decimal[] array)
        {
            decimal sum = 0;
            for (var i = 0; i < array.Length; i++)
            {
                sum += array[i];
            }
            return sum;
        }

        public decimal sumArray(decimal[] array, DateTime start, DateTime end)
        {
            decimal sum = 0;
            for (var s = start.Month - 1; s < end.Month; s++)
            {
                sum += array[s];
            }
            return sum;
        }

        public decimal[] divideArrays(decimal[] numerator, decimal[] denominator)
        {
            decimal[] values = new decimal[12];
            if (numerator == null)
            {
                numerator = values;
            }
            if (denominator == null)
            {
                denominator = values;
            }
            for (var i = 0; i < values.Length; i++)
            {
                if (denominator[i] != 0)
                {
                    values[i] = numerator[i] / denominator[i];
                }
            }

            return values;
        }

        public decimal[] roundUp(decimal[] array)
        {
            decimal[] values = new decimal[array.Length];
            var index = 0;
            foreach (var item in array)
            {
                if(item < 0)
                {
                    values[index] = item - (decimal)0.44;

                }
                else
                {
                    values[index] = item + (decimal)0.44;
                }
                index++;
            }

            return values;
        }

        public decimal[] divideArrayByValue(decimal[] numerator, decimal value)
        {
            decimal[] values = new decimal[12];
            if (numerator != null)
            {
                if (value == 0)
                {
                    value = 1;
                }
                for (var i = 0; i < values.Length; i++)
                {

                    values[i] = numerator[i] / value;
                }
            }
            

            return values;
        }

        public decimal[] divideArrayByValueIfSalary(decimal[] numerator, decimal[] salary, decimal value)
        {
            decimal[] values = new decimal[12];
            if (numerator == null)
            {
                numerator = values;
            }
            if (value == 0)
            {
                value = 1;
            }
            for (var i = 0; i < 12; i++)
            {
                if(salary[i] != 0)
                {
                    values[i] = numerator[i] / value;
                }
            }

            return values;
        }


        public decimal[] calculateDeductionsWithMaxValues(decimal max, decimal rate, decimal[] salary)
        {
            decimal[] values = new decimal[12];

                decimal total = 0;

                for (var i = 0; i < 12; i++)
                {
                    var deductionValue = salary[i] * rate;
                    var difference = max - total;

                    if (total <= max)
                    {
                        if (difference >= deductionValue)
                        {
                            values[i] = deductionValue;
                        }
                        else
                        {
                            values[i] = difference;
                        }
                    }

                    total += deductionValue;
                }  

            return values;
        }

        public decimal[] calculateDeductionsNoMax(decimal rate, decimal[] salary)
        {

            decimal[] values = new decimal[12];
            for (var i = 0; i < 12; i++)
            {
                values[i] = salary[i] * rate;
            }
            return values;
        }

        private decimal[] withoutMaxValue(decimal rate, decimal[] salary)
        {
            decimal[] values = new decimal[12];
            for (var i = 0; i < 12; i++)
            {
                values[i] = salary[i] * rate;
            }


                return values;
        }

        public decimal[] populateArray(decimal value)
        {
            decimal[] values = new decimal[12];
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = value;
            }
            return values;
        }

        public decimal[] studentSalary(decimal value, decimal[] workDays, DateTime start, DateTime end)
        {
            decimal[] values = new decimal[12];

            decimal[] monthlyHours = workHours(workDays);
            decimal termHours = sumArray(monthlyHours, start, end);
            if(termHours != 0)
            {
                decimal perHour = value / termHours;

                for (var s = start.Month - 1; s < end.Month; s++)
                {
                    values[s] = perHour * monthlyHours[s];
                }
            }

            return values;
        }

        

        public decimal[] monthlySalary(decimal value, DateTime start, DateTime end, int year)
        {
            var months = 12;
            var startD = 0;
            var endD = 12;

            if (end.Year == year)
            {
                endD = end.Month;
            }
            else if(end.Year < year)
            {
                return new decimal[12];
            }


            if (start.Year == year)
            {
                startD = start.Month - 1;
            }
            else if (start.Year > year)
            {
                return new decimal[12];
            }

            if(start.Year == year && end.Year == year)
            {
                months = end.Month - (start.Month - 1); 
            }
            decimal[] values = new decimal[12];
            for (var s = startD; s < endD; s++)
            {
                values[s] = value / months;
            }
            return values;
        }

        public decimal[] workHours(decimal[] workDays)
        {
            decimal[] values = new decimal[12];

            for(var i = 0; i < 12; i++)
            {
                values[i] = workDays[i] * (decimal)7.5;
            }
            return values;
        }


    }
}