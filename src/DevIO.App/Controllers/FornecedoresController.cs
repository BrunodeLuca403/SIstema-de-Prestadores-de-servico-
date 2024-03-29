﻿using Microsoft.AspNetCore.Mvc;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using AutoMapper;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using DevIO.App.Extensions;

namespace DevIO.App.Controllers
{
    [Authorize]
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepository fornecedorRepository, 
                                      IMapper mapper, 
                                      IFornecedorService fornecedorService,
                                      INotificador notificador) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("Lista-de-fornecedores")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos()));
        }

        [AllowAnonymous]
        [Route("dados-do-fornecedor/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {

            var fornecedorViewModel = await ObterFornecedorEndereco(id);
               
            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor", "Adc")]
        [Route("novo-fornecedor")]
        public IActionResult Create()
        {
            return View();
        }

        [ClaimsAuthorize("Fornecedor", "Adc")]
        [Route("novo-fornecedor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
        {
            if (ModelState.IsValid) return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Adicionar(fornecedor);

            return View(fornecedorViewModel);

        }

        [ClaimsAuthorize("Fornecedor", "Ed")]
        [Route("editar-fornecedor{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {

            var fornecedorViewModel = await ObterFornecedorProdutoEndereco(id);

            if (fornecedorViewModel == null)
            {
                return NotFound();
            }
            return View(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor", "Ed")]
        [Route("editar-fornecedor{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Edit(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Atualizar(fornecedor);

            return RedirectToAction(nameof(Index));

        }

        [ClaimsAuthorize("Fornecedor", "Exc")]
        [Route("deletar-fornecedor{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {

            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorViewModel);
        }

        [ClaimsAuthorize("Fornecedor", "Exc")]
        [Route("deletar-fornecedor{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
         
            var fornecedorViewModel = await ObterFornecedorEndereco(id);

            if (fornecedorViewModel == null)
            {
                return NotFound();
            }
            
            await _fornecedorService.Remover(id);

                
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [Route("obter-endereco-fornecedor{id:guid}")]
        public async Task<IActionResult> ObterEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);

            if(fornecedor == null) return NotFound();

            return PartialView("_DetalhesEndereco", fornecedor);

        }

        [ClaimsAuthorize("Fornecedor", "Ed")]
        [Route("atualizar-endereco-fornecedor{id:guid}")]
        public async Task<IActionResult> AtualizarEndereco(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id); 

            if(fornecedor == null)
            {
                return NotFound();
            }

            return PartialView("_AtualizarEndereco", new FornecedorViewModel { Endereco = fornecedor.Endereco});
        }

        [ClaimsAuthorize("Fornecedor", "Ed")] 
        [Route("atualizar-endereco-fornecedor{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizarEndereco(FornecedorViewModel fornecedorViewModel)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");

            if (!ModelState.IsValid)
            {
                return PartialView("_AtualizarEndereco", fornecedorViewModel);
            }

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(fornecedorViewModel.Endereco));

            var url = Url.Action("ObterEndereco", "Fornecedores",  new {id = fornecedorViewModel.Endereco.FornecedorId });

            return Json(new { sucess = true, url });
        }

        private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
        }

        private async Task<FornecedorViewModel> ObterFornecedorProdutoEndereco(Guid id)
        {
            return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
        }
    }
}
