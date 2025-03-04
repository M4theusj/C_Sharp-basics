using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RpgApi.Models;
using RpgApi.Models.Enuns;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonagensExemploController : ControllerBase
    {
         private static List<Personagem> personagens = new List<Personagem>()
        {
            new Personagem() { Id = 1, Nome = "Frodo", PontosVida=100, Forca=17, Defesa=23, Inteligencia=33, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 2, Nome = "Sam", PontosVida=100, Forca=15, Defesa=25, Inteligencia=30, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 3, Nome = "Galadriel", PontosVida=100, Forca=18, Defesa=21, Inteligencia=35, Classe=ClasseEnum.Clerigo },
            new Personagem() { Id = 4, Nome = "Gandalf", PontosVida=100, Forca=18, Defesa=18, Inteligencia=37, Classe=ClasseEnum.Mago },
            new Personagem() { Id = 5, Nome = "Hobbit", PontosVida=100, Forca=20, Defesa=17, Inteligencia=31, Classe=ClasseEnum.Cavaleiro },
            new Personagem() { Id = 6, Nome = "Celeborn", PontosVida=100, Forca=21, Defesa=13, Inteligencia=34, Classe=ClasseEnum.Clerigo },
            new Personagem() { Id = 7, Nome = "Radagast", PontosVida=100, Forca=25, Defesa=11, Inteligencia=35, Classe=ClasseEnum.Mago }
        };

        [HttpGet("GetByNome/{nome}")]
        public IActionResult GetByNome(string nome)
        {
            var personagem = personagens.FirstOrDefault(pe => pe.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase));

            if (personagem == null)
            {
                return NotFound(new { mensagem = $"Personagem com nome '{nome}' não encontrado." });
            }

            return Ok(personagem);
        }

        [HttpGet("GetClerigoMago")]
        public IActionResult GetClerigoMago()
        {
            var personagensFiltrados = personagens.Where(pe => pe.Classe != ClasseEnum.Cavaleiro );

            return Ok(personagensFiltrados);
        }

        [HttpGet("GetEstatisticas")]
        public IActionResult GetEstatisticas()
        {
            var numeroTotal = personagens.Count;
            var inteliTotal = personagens.Sum(pe => pe.Inteligencia);

            return Ok($"Personagens: '{numeroTotal}' Inteligencia: '{inteliTotal}'");
        }

        [HttpPost("PostValidacao")]
        public IActionResult AdicionarPersonagem([FromBody] Personagem novoPersonagem)
        {
            if (novoPersonagem.Defesa < 10 || novoPersonagem.Inteligencia > 30)
            {
                return BadRequest(new { mensagem = "Defesa abaixo de 10 ou inteligencia acima de 30!" });
            }

            return CreatedAtAction(nameof(GetByNome), new { nome = novoPersonagem.Nome }, novoPersonagem);
        }
        
        [HttpPost("PostValidacaoMago")]
        public IActionResult PostValidacaoMago([FromBody] Personagem novoPersonagem)
        {
            if (novoPersonagem.Classe == ClasseEnum.Mago && novoPersonagem.Inteligencia < 35)
            {
                return BadRequest(new { mensagem = "Mago muito burro!" });
            }

            return CreatedAtAction(nameof(GetByNome), new { nome = novoPersonagem.Nome }, novoPersonagem);
        }

        [HttpGet("GetByClasse/{classe}")]
        public IActionResult GetByClasse(string classe)
        {
            var personagensFiltrados = personagens.Where(pe => pe.Classe.ToString().Equals(classe, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(personagensFiltrados);
        }

    }
}

