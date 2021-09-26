using BusinessLogic.Exceptions;
using BusinessLogic.Validations;
using DataAccess.DAOs;
using System;

namespace BusinessLogic
{
    public class AccountManager
    {
        private Configuration configuration;

        private StringController stringController;

        AccountDAO accountDAO;

        private const int FirstPositionOfCellPhone = 0;

        public AccountManager()
        {
            configuration = new Configuration();
            stringController = new StringController();
            accountDAO = new AccountDAO();
        }
      
        public void AddValidAccount(string cellPhoneNumber,string selectedCountry)
        {
            if (selectedCountry == "Uruguay" && cellPhoneNumber[FirstPositionOfCellPhone] == '9')
            {
                cellPhoneNumber = '0' + cellPhoneNumber;
            }
            if (configuration.ValidateCellPhone(cellPhoneNumber, selectedCountry))
            {
                cellPhoneNumber = stringController.TakeOutSpaces(cellPhoneNumber);
                cellPhoneNumber = cellPhoneNumber.Replace("-", "");
                DataAccess.Entities.Account account = new DataAccess.Entities.Account
                {
                    CellPhoneNumber = cellPhoneNumber,
                    Country = selectedCountry
                    
                };
                AddIfNumberDoesNotExist(account);
            }
        }


        private void AddIfNumberDoesNotExist(DataAccess.Entities.Account account)
        {

            if (!accountDAO.Exist(account.CellPhoneNumber, account.Country))
            {
                accountDAO.Add(account);
            }
            else
            {
                throw new BusinessException("El numero ya esta regitrado en el sistema");
            }
        }
        public void AddBalance(string cellPhoneNumber,string balance, string selectedCountry)
        {
            if (cellPhoneNumber != string.Empty && cellPhoneNumber[FirstPositionOfCellPhone] == '9')
            {
                cellPhoneNumber = '0' + cellPhoneNumber;
            }
            if (configuration.ValidateCellPhone(cellPhoneNumber, selectedCountry))
            {
                AddBalanceIfNumberExists(cellPhoneNumber,balance, selectedCountry);
            }
        }

        private void AddBalanceIfNumberExists(string cellPhoneNumber,string balance, string selectedCountry)
        {
            if (accountDAO.Exist(cellPhoneNumber, selectedCountry))
            {
                AddIfBalanceIsCorrect(balance, cellPhoneNumber, selectedCountry);
            }
            else
            {
                throw new BusinessException("La cuenta especificada no esta registrada");
            }            
        }

        private void AddIfBalanceIsCorrect(string balance,string cellPhoneNumber, string selectedCountry)
        {
            if (balance != string.Empty)
            {
                if (stringController.IsANumber(balance))
                {
                    int result = Int32.Parse(balance);
                    accountDAO.AddBalance(cellPhoneNumber, result, selectedCountry);
                }
                else
                {
                    throw new BusinessException("El saldo ingresado no es un numero");
                }
            }
            else
            {
                throw new BusinessException( "El saldo ingresado es vacio");
            }
        }
    }
}
