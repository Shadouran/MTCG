using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    public interface ICardPackageRepository
    {
        void InsertCardPackage(CardPackage cardPackage);
        void RemoveCardPackage(CardPackage cardPackage);
        CardPackage GetRandomCardPackage();
        CardPackage GetFirstCardPackage();
    }
}
