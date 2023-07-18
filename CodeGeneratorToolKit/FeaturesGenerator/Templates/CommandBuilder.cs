using CodeGeneratorToolKit.FeaturesGenerator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGeneratorToolKit.FeaturesGenerator.Templates
{
    public static class CommandBuilder
    {
        public static string? CreateCommand(TemplateViewModel templateModel, GeneratorViewModel generatorModel)
        {
            return
        $@"
            {generatorModel?.HandlerVM?.CommandType}{generatorModel?.FileName}Validator validator = new();
            var isValid = await validator.ValidateAsync(request, cancellationToken);
            if (isValid.Errors.Any())
                return new {templateModel?.ResponseTypeName}(FeedBackCode.ValidationNotValid, isValid.Errors);

            var model = Mapper.Map<{generatorModel?.FileName}>(request);
            UnitOfWork.{generatorModel?.FileName}.Insert(model);

            int isSaved = await UnitOfWork.{generatorModel?.FileName}.SaveChangesAsync(cancellationToken);
            if (isSaved <= 0)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NotCreated);

            var viewModel = await UnitOfWork.{generatorModel?.FileName}.GetVMAsync(x => x.Id == model.Id);
            return new {templateModel?.ResponseTypeName}(FeedBackCode.Created, viewModel);
        ";
        }
        public static string? ActiveCommand(TemplateViewModel templateModel, GeneratorViewModel generatorModel)
        {
            return
        $@"
            if (request is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NullOrEmpty);

            if (request.Id is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NullOrEmpty);

            var getRow = await UnitOfWork.{generatorModel?.FileName}.GetAsync(x => x.Id == request.Id);
            if (getRow is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NotFound);

            var model = Mapper.Map<{generatorModel?.HandlerVM?.CommandType}{generatorModel?.FileName}Command, {generatorModel?.FileName}>(request, getRow);
            UnitOfWork.{generatorModel?.FileName}.Update(model);

            int isSaved = await UnitOfWork.{generatorModel?.FileName}.SaveChangesAsync(cancellationToken);
            if (isSaved <= 0)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NotActived);

            var viewModel = Mapper.Map<{generatorModel?.ClassVM?.ClassName}{generatorModel?.FileName}ViewModel>(model);
            return new {templateModel?.ResponseTypeName}(FeedBackCode.Actived, viewModel);
        ";
        }
        public static string? UpdateCommand(TemplateViewModel templateModel, GeneratorViewModel generatorModel)
        {
            return
        $@"
            {generatorModel?.HandlerVM?.CommandType}{generatorModel?.FileName}Validator validator = new();
            var isValid = await validator.ValidateAsync(request, cancellationToken);
            if (isValid.Errors.Any())
                return new {templateModel?.ResponseTypeName}(FeedBackCode.ValidationNotValid, isValid.Errors);

            var getRow = await UnitOfWork.{generatorModel?.FileName}.GetAsync(x => x.Id == request.Id);
            if (getRow is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NotFound);

            var model = Mapper.Map<{generatorModel?.HandlerVM?.CommandType}{generatorModel?.FileName}Command, {generatorModel?.FileName}>(request, getRow);
            UnitOfWork.{generatorModel?.FileName}.Update(model);

            int isSaved = await UnitOfWork.{generatorModel?.FileName}.SaveChangesAsync(cancellationToken);
            if (isSaved <= 0)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NotUpdated);

            var viewModel = Mapper.Map<{generatorModel?.ClassVM?.ClassName}{generatorModel?.FileName}ViewModel>(model);
            return new {templateModel?.ResponseTypeName}(FeedBackCode.Updated, viewModel);
        ";
        }
        public static string? DeleteCommand(TemplateViewModel templateModel, GeneratorViewModel generatorModel)
        {
            return
        $@"
            if (request.Id is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NullOrEmpty);

            var getRow = await UnitOfWork.{generatorModel?.FileName}.GetAsync(x => x.Id == request.Id);
            if (getRow is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NullOrEmpty);

            var model = Mapper.Map<{generatorModel?.HandlerVM?.CommandType}{generatorModel?.FileName}Command, {generatorModel?.FileName}>(request, getRow);
            UnitOfWork.{generatorModel?.FileName}.Update(model);
           
            int isSaved = await UnitOfWork.{generatorModel?.FileName}.SaveChangesAsync(cancellationToken);
            if (isSaved <= 0)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NotDeleted);

            var viewModel = Mapper.Map<{generatorModel?.ClassVM?.ClassName}{generatorModel?.FileName}ViewModel>(model);
            return new {templateModel?.ResponseTypeName}(FeedBackCode.Deleted, viewModel);
        ";
        }
        public static string? ArchiveCommand(TemplateViewModel templateModel, GeneratorViewModel generatorModel)
        {
            return
        $@"
            if (request is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NullOrEmpty);

            if (request.Id is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NullOrEmpty);

            var getRow = await UnitOfWork.{generatorModel?.FileName}.GetAsync(x => x.Id == request.Id);
            if (getRow is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NotFound);

            var model = Mapper.Map<{generatorModel?.HandlerVM?.CommandType}{generatorModel?.FileName}Command, {generatorModel?.FileName}>(request, getRow);
            UnitOfWork.{generatorModel?.FileName}.Update(model);

            int isSaved = await UnitOfWork.{generatorModel?.FileName}.SaveChangesAsync(cancellationToken);
            if (isSaved <= 0)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NotArchived);

            var viewModel = Mapper.Map<{generatorModel?.ClassVM?.ClassName}{generatorModel?.FileName}ViewModel>(model);
            return new {templateModel?.ResponseTypeName}(FeedBackCode.Archived, viewModel);
        ";
        }
        public static string? RecoverCommand(TemplateViewModel templateModel, GeneratorViewModel generatorModel)
        {
            return
        $@"
            if (request is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NullOrEmpty);

            if (request.Id is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NullOrEmpty);

            var getRow = await UnitOfWork.{generatorModel?.FileName}.GetAsync(x => x.Id == request.Id);
            if (getRow is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NotFound);

            var model = Mapper.Map<{generatorModel?.HandlerVM?.CommandType}{generatorModel?.FileName}Command, {generatorModel?.FileName}>(request, getRow);
            UnitOfWork.{generatorModel?.FileName}.Update(model);

            int isSaved = await UnitOfWork.{generatorModel?.FileName}.SaveChangesAsync(cancellationToken);
            if (isSaved <= 0)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NotRecoverd);

            var viewModel = Mapper.Map<{generatorModel?.ClassVM?.ClassName}{generatorModel?.FileName}ViewModel>(model);
            return new {templateModel?.ResponseTypeName}(FeedBackCode.Recoverd, viewModel);
        ";
        }
        public static string? RestoreCommand(TemplateViewModel templateModel, GeneratorViewModel generatorModel)
        {
            return
        $@"
            if (request is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NullOrEmpty);

            if (request.Id is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NullOrEmpty);

            var getRow = await UnitOfWork.{generatorModel?.FileName}.GetAsync(x => x.Id == request.Id);
            if (getRow is null)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NotFound);

            var model = Mapper.Map<{generatorModel?.HandlerVM?.CommandType}{generatorModel?.FileName}Command, {generatorModel?.FileName}>(request, getRow);
            UnitOfWork.{generatorModel?.FileName}.Update(model);

            int isSaved = await UnitOfWork.{generatorModel?.FileName}.SaveChangesAsync(cancellationToken);
            if (isSaved <= 0)
                return new {templateModel?.ResponseTypeName}(FeedBackCode.NotRestored);

            var viewModel = Mapper.Map<{generatorModel?.ClassVM?.ClassName}{generatorModel?.FileName}ViewModel>(model);
            return new {templateModel?.ResponseTypeName}(FeedBackCode.Restored, viewModel);
        ";
        }
    }
}
