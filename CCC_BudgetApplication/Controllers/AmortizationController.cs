using Application.Models;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class AmortizationController : ObjectInstanceController
    {
        // GET: Amortization
        public ActionResult Index()
        {
            return View();
        }


        //------------------------------------------
        //ActionResult
        //------------------------------------------
        public ActionResult AmortizationCost(int poolID = 0)
        {
            List<AmortizationViewModel> list = new List<AmortizationViewModel>();
            try
            {
                var pools = findPool(poolID);
                foreach (var p in pools)
                {
                    list.Add(BuildModel(p));
                }
                list.Add(total(list));
            }
            catch(Exception ex)
            {
                log.Warn("amortization cost failed", ex);
            }
            
            return View(list);
        }

        public ActionResult DeferredAmortization(int poolID = 0)
        {
            List<AmortizationViewModel> list = new List<AmortizationViewModel>();

            return View(list);
        }

        public ActionResult DeferredAmortizationCost(int poolID = 0)
        {
            List<AmortizationViewModel> list = new List<AmortizationViewModel>();
            try
            {
                var pools = findDeferredPool(poolID);
                foreach (var p in pools)
                {
                    list.Add(BuildModel(p));
                }
                list.Add(total(list));
            }
            catch (Exception ex)
            {
                log.Warn("amortization cost failed", ex);
            }

            return View(list);
        }

        public ActionResult AccumulatedAmortization(int poolID = 0)
        {
            List<AmortizationViewModel> list = new List<AmortizationViewModel>();

            return View(list);
        }

        public ActionResult Additions(int poolID = 0)
        {
            List<AmortizationViewModel> list = new List<AmortizationViewModel>();

            return View(list);
        }

        public ActionResult Disposals(int poolID = 0)
        {
            List<AmortizationViewModel> list = new List<AmortizationViewModel>();

            return View(list);
        }

        //------------------------------------------
        //Actions
        //------------------------------------------

        private AmortizationViewModel BuildModel(Pool pool)
        {
            AmortizationViewModel item = new AmortizationViewModel();
            try
            {
                if (pool != null)
                {
                    item.Name = pool.Name;
                    item.AccountNum = (int)pool.AccountNum;
                    item.Rate = pool.DepreciationRate;
                    item.StraightLine = (bool)pool.StraightLine;
                    item.SourceID = pool.PoolID;
                    if (pool.C_isDeferred)
                    {
                        var amortization = getDeferredAmortization(pool.PoolID).SingleOrDefault();
                        if (amortization != null)
                        {
                            item = buildModel(item, amortization);
                        }
                    }
                    else
                    {
                        var amortization = getAmortization(pool.PoolID).SingleOrDefault();
                        if (amortization != null)
                        {
                            item = buildModel(item, amortization);
                        }
                    }


                }
            }
            catch(Exception ex)
            {
                log.Warn("Amortization view model build failed", ex);
            }
            
            

            return item;
        }

        private AmortizationViewModel buildModel(AmortizationViewModel item, DeferredAmortization amortization)
        {
            item.PoolBalance = amortization.PoolBalance;
            item.AmortizationBalance = amortization.AccumulatedAmortization;
            var add = getDeferredAdditions(amortization.DeferredAmortizationID);
            if (add != null)
            {
                item.AdditionList = add;
                item.AdditionValue = add.Sum(x => x.Value);
            }
            var dis = getDeferredDisposals(amortization.DeferredAmortizationID);
            if (dis != null)
            {
                //item.DisposalList = dis;
                //item.DisposalValue = dis.Sum(x => x.Value);

            }
            item.CurrentAfterRate = CurrentAmortization(item, item.StraightLine);
            item.AdditionAfterRate = AmortizationAddition(item, item.StraightLine);
            item.Provision = provision(item, item.StraightLine);
            item.Values = monthlyAmortization(amortization.DeferredAmortizationID, item.Provision);
            item.Year = YEAR;
            item.ViewClass = "";
            return item;
        }

        private AmortizationViewModel buildModel(AmortizationViewModel item, Amortization amortization)
        {
            item.PoolBalance = amortization.PoolBalance;
            item.AmortizationBalance = amortization.AccumulatedAmortization;
            var add = getAdditions(amortization.AmortizationID);
            if (add != null)
            {
                item.AdditionList = add;
                item.AdditionValue = add.Sum(x => x.Value);
            }
            var dis = getDisposals(amortization.AmortizationID);
            if (dis != null)
            {
                //item.DisposalList = dis;
                //item.DisposalValue = dis.Sum(x => x.Value);

            }
            item.CurrentAfterRate = CurrentAmortization(item, item.StraightLine);
            item.AdditionAfterRate = AmortizationAddition(item, item.StraightLine);
            item.Provision = provision(item, item.StraightLine);
            item.Values = monthlyAmortization(amortization.AmortizationID, item.Provision);
            item.Year = YEAR;
            item.ViewClass = "";
            return item;
        }
        private decimal CurrentAmortization(AmortizationViewModel item, bool straightLine = false)
        {
            //check if straight line
            //if straight line / months
            var current = item.PoolBalance;
            try
            {
                var r = item.Rate;
                if (!straightLine)
                {
                    if (item.Rate > 1)
                    {
                        r = r / 100;
                    }
                    current = (item.PoolBalance - item.AmortizationBalance) * r;
                }
                else
                {
                    //convert rate from months to years
                    r /= 12;

                    current /= r;
                }
            }
            catch(Exception ex)
            {
                log.Info("current amortization failed", ex);
            }
            

            return current;
        }

        private decimal AmortizationAddition(AmortizationViewModel item, bool straightLine = false)
        {

            //check if straight line
            //if straight line / months
            var addition = (item.AdditionValue * (decimal)0.5);

            try
            {
                var r = item.Rate;
                if (!straightLine)
                {
                    if (item.Rate > 1)
                    {
                        r = r / 100;
                    }
                    addition *= r;
                }
                else
                {
                    //convert rate from months to years
                    r /= 12;

                    addition /= r;
                }

            }
            catch(Exception ex)
            {
                log.Info("amortization addition failed", ex);
            }
            

            return addition;
        }

        private decimal provision(AmortizationViewModel item, bool straightLine = false)
        {
            var current = CurrentAmortization(item, straightLine);
            var addition = AmortizationAddition(item, straightLine);

            return current + addition;
        }

        //-----------
        //UNCOMMENT TO MAKE CALCULATIONS INCLUDE DATES OF NEW ACQUISITION
        //------------
        private decimal[] monthlyAmortization(int amortizationID, decimal provision)
        {
            //var additions = db.Additions.Where(x => x.AmortizationID == amortizationID && x.Date.Year == YEAR).Select(x => x);

            decimal[] values = new decimal[12];
            decimal sum = 0;
            int index = 0;
            //decimal provision = 0;
            try
            {
                for (var month = 12; month > 0; month--)
                {
                    //var p = additions.Where(x => x.Date.Month == index + 1).Select(x => x.Value);
                    //if (p != null)
                    //{
                    //provision += sumQueryable(p);
                    var temp = (provision - sum) / month;
                    sum += temp;

                    values[index] = temp;

                    // }

                    index++;
                }
            }
            catch(Exception ex)
            {
                log.Debug("monthly amortization", ex);
            }
            

            return values;
        }

        private decimal sumQueryable(IQueryable<decimal> data)
        {
            decimal value = 0;

            foreach(var d in data)
            {
                value += d;
            }

            return value;
        }



        //------------------------------------------
        //QUERIES
        //------------------------------------------
        public IQueryable<Pool> findPool(int id)
        {
            if (id == 0)
            {
                return db.Pools.Where(x => !x.C_isDeferred).Select(x => x);
            }
            else
            {
                return db.Pools.Where(x => x.PoolID == id).Select(x => x);
            }
        }

        public IQueryable<Pool> findDeferredPool(int id)
        {
            if (id == 0)
            {
                return db.Pools.Where(x => x.C_isDeferred).Select(x => x);
            }
            else
            {
                return db.Pools.Where(x => x.PoolID == id).Select(x => x);
            }
        }

        public IQueryable<Amortization> getAmortization(int poolID)
        {
            return db.Amortizations.Where(x => x.PoolID == poolID && x.Year == YEAR).Select(x => x);
        }
        public IQueryable<DeferredAmortization> getDeferredAmortization(int poolID)
        {
            return db.DeferredAmortizations.Where(x => x.PoolID == poolID && x.Year == YEAR).Select(x => x);
        }
        public IQueryable<Addition> getAdditions(int amortizationID)
        {
            return db.Additions.Where(x => x.AmortizationID == amortizationID && x.Date.Year == YEAR).Select(x => x);
        }
        public IQueryable<Addition> getDeferredAdditions(int amortizationID)
        {
            return db.Additions.Where(x => x.DeferredAmortizationID == amortizationID && x.Date.Year == YEAR).Select(x => x);
        }
        public IQueryable<Disposal> getDisposals(int amortizationID)
        {
            return db.Disposals.Where(x => x.AmortizationID == amortizationID && x.Date.Year == YEAR).Select(x => x);
        }
        public IQueryable<Disposal> getDeferredDisposals(int amortizationID)
        {
            return db.Disposals.Where(x => x.DeferredAmortizationID == amortizationID && x.Date.Year == YEAR).Select(x => x);
        }

        private AmortizationViewModel total(List<AmortizationViewModel> data)
        {
            AmortizationViewModel item = new AmortizationViewModel();
            try
            {
                item.Name = "Total";
                decimal[] values = new decimal[12];
                foreach (var d in data)
                {
                    item.PoolBalance += d.PoolBalance;
                    item.AmortizationBalance += d.AmortizationBalance;
                    item.AdditionValue += d.AdditionValue;
                    item.AdditionAfterRate += d.AdditionAfterRate;
                    item.CurrentAfterRate = d.CurrentAfterRate;
                    item.Provision += d.Provision;
                    values = ARRAYSERVICES.combineArrays(values, d.Values);
                }
                item.ViewClass = "highlight";
                item.Year = YEAR;
                item.Values = values;
            }
            catch(Exception ex)
            {
                log.Warn("View model build failed", ex);
            }
            
            return item;
        }

    }
}
