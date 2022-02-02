using System.Net;

namespace Microsoft.AspNetCore.Mvc;

public static class ControllerExtensions {
    [NonAction]
    public static ViewResult BadRequestView(this Controller controller, (string key, string errorMessage)? modelError) =>
        controller.AddModelErrorIfModelErrorHasValue(modelError).CreateViewWithHttpStatusCode(null, HttpStatusCode.BadRequest);

    [NonAction]
    public static ViewResult BadRequestView(this Controller controller, string viewName, object model, (string key, string errorMessage)? modelError) =>
        controller
            .AddModelErrorIfModelErrorHasValue(modelError)
            .AddModelInViewDataIfModelHasValue(model)
            .CreateViewWithHttpStatusCode(viewName, HttpStatusCode.BadRequest);

    [NonAction]
    public static ViewResult BadRequestView(this Controller controller, object model, (string key, string errorMessage)? modelError) =>
        controller
            .AddModelErrorIfModelErrorHasValue(modelError)
            .AddModelInViewDataIfModelHasValue(model).
            CreateViewWithHttpStatusCode(null, HttpStatusCode.BadRequest);

    [NonAction]
    public static ViewResult BadRequestView(this Controller controller, string viewName, (string key, string errorMessage)? modelError) =>
        controller
            .AddModelErrorIfModelErrorHasValue(modelError)
            .CreateViewWithHttpStatusCode(viewName, HttpStatusCode.BadRequest);

    [NonAction]
    public static ViewResult CreateHttpGetViewResult(this Controller controller, object? model, (string key, string errorMessage)? modelError = default) =>
         model switch {
             IEnumerable<object> e when e.Any() => controller.AddModelErrorIfModelErrorHasValue(modelError).View(e),
             IEnumerable<object> e when !e.Any() => controller.AddModelErrorIfModelErrorHasValue(modelError).CreateViewWithHttpStatusCode(null, HttpStatusCode.NotFound),
             object => controller.AddModelErrorIfModelErrorHasValue(modelError).View(model),
             null => controller.AddModelErrorIfModelErrorHasValue(modelError).CreateViewWithHttpStatusCode(null, HttpStatusCode.NotFound)
         };

    internal static Controller AddModelErrorIfModelErrorHasValue(this Controller controller, (string key, string errorMessage)? modelError) {
        if (modelError is not null) {
            controller.ModelState.AddModelError(modelError.Value.key, modelError.Value.errorMessage);
        }

        return controller;
    }

    internal static Controller AddModelInViewDataIfModelHasValue(this Controller controller, object model) {
        if (model is not null) {
            controller.ViewData.Model = model;
        }

        return controller;
    }

    internal static ViewResult CreateViewWithHttpStatusCode(this Controller controller, string? viewName, HttpStatusCode statusCode) =>
        new() {
            ViewName = viewName,
            ViewData = controller.ViewData,
            TempData = controller.TempData,
            StatusCode = (int)statusCode
        };
}
