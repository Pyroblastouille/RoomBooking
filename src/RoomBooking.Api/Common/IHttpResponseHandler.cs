using Microsoft.AspNetCore.Mvc;
using RoomBooking.Application.Common;
namespace RoomBooking.Api.Interfaces;

public interface IHttpResponseHandler
{
    /// <summary>
    /// Traite un résultat de service réussi.
    /// </summary>
    /// <typeparam name="T">Type des données</typeparam>
    /// <param name="data">Les données à retourner</param>
    /// <param name="location">URL de la ressource créée (pour POST)</param>
    /// <returns>Réponse HTTP appropriée (200 ou 201)</returns>
    IActionResult HandleSuccess<T>(T data, string? location = null);

    /// <summary>
    /// Traite un résultat de service échoué.
    /// </summary>
    /// <typeparam name="T">Type des données</typeparam>
    /// <param name="result">Le ServiceResult échoué</param>
    /// <param name="code">Le code d'erreur à renvoyé</param>
    /// <returns>Réponse HTTP appropriée (400, 404, 409...)</returns>
    IActionResult HandleFailure<T>(ServiceResult<T> result,int code = 0);
}