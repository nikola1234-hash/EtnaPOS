using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Media;
using EtnaPOS.DAL.DataAccess;
using EtnaPOS.DAL.Models;

namespace EtnaPOS.Models
{
    public class Table : BaseModel
    {
        private EtnaDbContext _db => App.GetService<EtnaDbContext>()!;

        public string TableName { get; }
        public int Id { get; }
        public List<Document> Documents { get; }
        [DefaultValue(0)]
        public decimal TotalPrice { get; }
        public bool ShowPrice { get; }
        public Brush TableBackground
        {
            get
            {
                if(Documents.Count() > 1)
                {
                    throw new Exception("More than one Document is open for given table. Fatal Error");
                }
                if (Documents.FirstOrDefault() != null)
                {
                    
                    return new SolidColorBrush(Color.FromRgb(220, 20, 60));
                }
                else
                {
                    return new SolidColorBrush(Color.FromArgb(255, 144, 238, 144));
                }

            }
        }

        public Table(EtnaPOS.DAL.Models.Table table)
        {
            if (table == null)
                throw new NullReferenceException();

            TableName = table.TableName;
            Id = table.Id;
            Documents = _db.Documents.Where(s => s.Date == WorkDay.Date && s.TableId == Id && s.IsOpen).Include(o => o.Orders).ToList();
            if (Documents != null && Documents.Count > 0)
            {
                if (Documents.FirstOrDefault().IsOpen && Documents.FirstOrDefault().Orders != null)
                {
                    TotalPrice = (decimal) Documents.FirstOrDefault()!.Orders.Sum(s => s.Price);
                }
            }
            else
            {
                TotalPrice = 0M;
            }

            ShowPrice = TotalPrice > 0 ? true : false;
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
