using Api.Data.Collections;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/series/")]
    public class InfectadoController : ControllerBase
    {
        Data.MongoDB _mongoDB;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.MongoDB mongoDB)
        {
            _mongoDB = mongoDB;
            _infectadosCollection = _mongoDB.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }
        ///<summary>
        ///Registro de infectados
        ///</summary>
        ///<param name="dto">View Model de Registro do Infectado</param>
        ///<returns></returns>
        [HttpPost]
        [Route("RegistrarInfectado")]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);

            _infectadosCollection.InsertOne(infectado);
            
            return StatusCode(201, "Infectado adicionado com sucesso");
        }
        ///<summary>
        ///Obtem o registro do infectado
        ///</summary>
        ///<returns></returns>
        [HttpGet]
        [Route("ObterInfectado")]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();
            
            return Ok(infectados);
        }
        [HttpPut]
        [Route("Atualizar")]
        public ActionResult AtualizarInfectados([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);
            _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(_ =>
            _.DataNascimento == dto.DataNascimento),
            Builders<Infectado>.Update.Set("sexo", dto.Sexo));

            return Ok("Atualização feita com sucesso");
        }
        [HttpDelete]
        [Route("Deletar")]
        public ActionResult Detele(DeleteViewModelInput deleteViewModelInput)
        {
            //var infectado = new Infectado(dto.DataNascimento, dto.Sexo, dto.Latitude, dto.Longitude);
            _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(_ =>
            _.DataNascimento == deleteViewModelInput.DataNascimentoDelete));

            return Ok("Dados Apagados com sucesso");
        }
    }
}

//mongodb+srv://api:<password>@cluster0.eczjj.mongodb.net/myFirstDatabase?retryWrites=true&w=majority
