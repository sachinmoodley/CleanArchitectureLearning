using Domain;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Presenters
{
    public interface IActionResultPresenter : IPresenter
    {
        IActionResult Render();
    }
}