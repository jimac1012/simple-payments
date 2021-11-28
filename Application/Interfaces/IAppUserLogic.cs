using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAppUserLogic
    {
        List<AppUserModel> GetData();

        void SaveData(AppUserModel userModel);

        void Dispose();
    }
}
