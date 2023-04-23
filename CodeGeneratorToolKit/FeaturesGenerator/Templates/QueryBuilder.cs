using   CodeGeneratorToolKit.FeaturesGenerator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGeneratorToolKit.FeaturesGenerator.Templates
{
    public static class QueryBuilder
    {
        public static string? DetailQuery(TemplateViewModel templateModel, GeneratorViewModel generatorModel)
        {
            return
        $@"
            if(request is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NullOrEmpty);

            if (request.Id is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NullOrEmpty);

            var getRow = await UnitOfWork.{generatorModel?.FileName}.GetVMAsync(x => x.Id == request.Id);
            if (getRow is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NotFound);

            return new {templateModel?.ResponseTypeName}(FeedBackCode.OK, getRow);
        ";
        }
        public static string? ListQuery(TemplateViewModel templateModel, GeneratorViewModel generatorModel)
        {
            return
        $@"
            {generatorModel?.ClassVM?.ClassName}{generatorModel?.FileName}Validator validator = new();
            var isValid = await validator.ValidateAsync(request, cancellationToken);
            if (isValid.Errors.Any())
                return new {templateModel?.ResponseTypeName}(FeedBackCode.ValidationNotValid, isValid.Errors);

            var getList = await UnitOfWork.{generatorModel?.FileName}.GetAllAsync(request.PageIndex, request.PageSize, 
                filter: x => x.ActionStatus != ActionStatus.Deleted && x.ActionStatus != ActionStatus.Archived && x.ActiveStatus, 
                orderBy: x => x.OrderByDescending(xx => xx.CreatedDate));
            
            if (!getList.Any())
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NotFound);

            return new {templateModel?.ResponseTypeName}(FeedBackCode.OK, getList);
        ";
        }
        public static string? ActionStatusListQuery(TemplateViewModel templateModel, GeneratorViewModel generatorModel)
        {
            return
        $@"
            {generatorModel?.ClassVM?.ClassName}{generatorModel?.FileName}Validator validator = new();
            var isValid = await validator.ValidateAsync(request, cancellationToken);
            if (isValid.Errors.Any())
                return new {templateModel?.ResponseTypeName}(FeedBackCode.ValidationNotValid, isValid.Errors);

            var getList = await UnitOfWork.{generatorModel?.FileName}.GetAllAsync(request.PageIndex, request.PageSize, 
                filter: x => x.ActionStatus == request.ActionStatusId, 
                orderBy: x => x.OrderByDescending(xx => xx.CreatedDate){(generatorModel?.HandlerVM?.HasIncludesNames == true ? generatorModel.HandlerVM.IncludesNames:string.Empty)});

            if (!getList.Any())
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NotFound);

            return new {templateModel?.ResponseTypeName}(FeedBackCode.OK, getList);
        ";
        }
        
    }
}
