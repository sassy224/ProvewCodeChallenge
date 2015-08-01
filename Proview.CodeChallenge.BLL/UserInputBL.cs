using Proview.CodeChallenge.BLL.ControlObject;
using Proview.CodeChallenge.BLL.Extension;
using Proview.CodeChallenge.DAL;
using Proview.CodeChallenge.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proview.CodeChallenge.BLL
{
    /// <summary>
    /// This class is used to manipulate business logic for object UserInput
    /// </summary>
    public class UserInputBL
    {
        /// <summary>
        /// The UnitOfWork class that will hold the data context to be used
        /// </summary>
        private UnitOfWork unitOfWork = new UnitOfWork();
        
        /// <summary>
        /// Get objects of type UserInput
        /// </summary>
        /// <param name="paging">Object that holds data for filters and paging UserInput objects</param>
        /// <returns>Object of type ResultModel</returns>
        public ResultModel GetUserInputs(PagingModel paging){
            var userInputs = unitOfWork.UserInputRepository.GetAll();

            //Filter
            var filteredUserInputs = userInputs.Where(u => u.Id.ToString().Contains(paging.SearchString)
                                         || u.Expression.Contains(paging.SearchString)
                                         || u.Result.Contains(paging.SearchString));
            //Order
            var userInputsWithOrder = filteredUserInputs.OrderBy(paging.SortOn, paging.SortReversed == "true");

            //Paging
            var userInputsWithPaging = userInputsWithOrder.Skip(paging.PageIndex * paging.PageSize)
                                                  .Take(paging.PageSize);

            //Retun result to display
            var result = new List<System.Object>();
            foreach (var uInput in userInputsWithPaging)
            {
                result.Add(new UserInput
                {
                    Id = uInput.Id,
                    Expression = uInput.Expression,
                    Result = uInput.Result
                });
            }

            return new ResultModel(result, userInputsWithOrder.Count());
        }

        /// <summary>
        /// Add an UserInput object to database 
        /// </summary>
        /// <param name="expression">Value of the Expression column</param>
        /// <param name="expressionResult">Value of the Result column</param>
        /// <returns>true if added successfully, false otherwise</returns>
        public Boolean AddUserInput(String expression, String expressionResult)
        {
            try
            {
                UserInput uInput = new UserInput();
                uInput.Expression = expression;
                uInput.Result = expressionResult;

                unitOfWork.UserInputRepository.Insert(uInput);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                //TODO: log here
                return false;
            }

            return true;
        }

        /// <summary>
        /// Delete an UserInput object from database
        /// </summary>
        /// <param name="uId">Id of the UserInput object</param>
        /// <returns>true if delete successfully, false otherwise</returns>
        public Boolean DeleteUserInput(int uId)
        {
            try
            {
                UserInput uInput = unitOfWork.UserInputRepository.GetById(uId);
                unitOfWork.UserInputRepository.Delete(uInput);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                //TODO: log here
                return false;
            }

            return true;
        }
    }
}
