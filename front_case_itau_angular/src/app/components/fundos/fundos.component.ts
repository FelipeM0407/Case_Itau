import { Component, OnInit, TemplateRef } from '@angular/core';
import { FundosService } from '../../fundos.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Fundo } from '../../Fundo';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-fundos',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './fundos.component.html',
  styleUrl: './fundos.component.css'
})
export class FundosComponent implements OnInit {
  form: any;
  titleForm: string = '';
  fundos: Fundo[] = [];
  fundoNome: string = '';
  fundoCodigo: string = '';
  fundoPatrimonio: number = 0;

  visibilityTable: boolean = true;
  visibilityForm: boolean = false;

  modalRef?: BsModalRef;
  modalRefPatrimonio?: BsModalRef;

  tipoFundoDict: { [key: number]: string } = {
    1: 'RENDA FIXA',
    2: 'ACOES',
    3: 'MULTI MERCADO'
  };
  tipoFundoKeys = Object.keys(this.tipoFundoDict).map(key => Number(key));

  constructor(private fundosService: FundosService,
    private modalService: BsModalService
  ) { }

  ngOnInit(): void {
    this.fundosService.GetAll().subscribe(result => {
      this.fundos = result;
    })

    this.titleForm = 'Novo Fundo';
    this.form = new FormGroup({
      codigo: new FormControl(null),
      nome: new FormControl(null),
      cnpj: new FormControl(null),
      codigo_Tipo: new FormControl(null),
      patrimonio: new FormControl(null)
    });
  }

  ShowFormRegister(): void {
    this.visibilityTable = false;
    this.visibilityForm = true;

    this.titleForm = 'Novo Fundo';
    this.form = new FormGroup({
      nome: new FormControl(null),
      cnpj: new FormControl(null),
      codigo: new FormControl(null),
      codigo_Tipo: new FormControl(null),
      patrimonio: new FormControl(null)
    });
  }

  validateNumber(event: KeyboardEvent) {
    const pattern = /[0-9]/;
    const inputChar = String.fromCharCode(event.keyCode);
  
    if (!pattern.test(inputChar)) {
      event.preventDefault();
    }
  }

  SendForm(): void {
    const fundo: Fundo = this.form.getRawValue();
    const desabilitado = this.form.get('codigo')?.disabled;

    if ((fundo.codigo !== undefined && fundo.codigo != "") && desabilitado) {
      this.fundosService.UpdateFundo(fundo).subscribe(x => {
        this.visibilityForm = false;
        this.visibilityTable = true;

        alert("Fundo atualizado com sucesso !");

        this.fundosService.GetAll().subscribe(res => {
          this.fundos = res;
        });
      });

    } else {

      this.fundosService.SaveFundo(fundo).subscribe({
        next: (result) => {
          this.visibilityForm = false;
          this.visibilityTable = true;
          alert("Fundo salvo com sucesso!");
      
          this.fundosService.GetAll().subscribe(res => {
            this.fundos = res;
          });
        },
        error: (errorResponse) => {
          if (errorResponse.status === 409) {
            alert("Código do Fundo já existente!");
          } else if (errorResponse.status === 500) {
            alert("Erro ao salvar o fundo. Tente novamente.");
          } else {
            alert("Ocorreu um erro inesperado. Tente novamente.");
          }
        }
      });
      
    }
  }

  SendNovoPatrimonio(): void {
    const fundo: Fundo = this.form.getRawValue();
    const fundoCodigo= this.fundoCodigo;
    
    this.fundosService.AddFundo(fundoCodigo, fundo.patrimonio || 0).subscribe(x => {
      alert("Patrimonio atualizado com sucesso !");

      this.fundosService.GetAll().subscribe(res => {
        this.fundos = res;
        this.modalRefPatrimonio?.hide();
      });
    });
  }

  Back(): void {
    this.visibilityTable = true;
    this.visibilityForm = false;
  }

  MostrarFormularioAtualizacao(fundoCodigo: string): void {
    this.visibilityTable = false;
    this.visibilityForm = true;

    this.fundosService.GetByCodigo(fundoCodigo).subscribe(result => {
      this.titleForm = `Atualizar ${result.nome}`;

      this.form = new FormGroup({
        codigo: new FormControl({ value: result.codigo, disabled: true }),
        nome: new FormControl(result.nome),
        cnpj: new FormControl(result.cnpj),
        codigo_Tipo: new FormControl(result.codigo_Tipo),
        patrimonio: new FormControl(result.patrimonio)
      });
    });

  }

  MostrarConfirmacaoExclusao(fundoCodigo: string, fundoNome: string, contentModal: TemplateRef<any>): void {
    this.modalRef = this.modalService.show(contentModal);
    this.fundoCodigo = fundoCodigo;
    this.fundoNome = fundoNome;
  };

  MostrarModalAdicaoPatrimonio(fundoCodigo: string, fundoNome: string, contentModalPatrimonio: TemplateRef<any>): void {
    this.modalRefPatrimonio = this.modalService.show(contentModalPatrimonio);
    this.fundoCodigo = fundoCodigo;
    this.fundoNome = fundoNome;

    this.form = new FormGroup({
      codigo: new FormControl(null),
      nome: new FormControl(null),
      cnpj: new FormControl(null),
      codigo_Tipo: new FormControl(null),
      patrimonio: new FormControl(null)
    });
  };

  DeleteFundo(fundoCodigo: string) {
    this.fundosService.DeleteFundo(fundoCodigo).subscribe(result => {
      this.modalRef?.hide();
      alert("Fundo deletado!");
      this.fundosService.GetAll().subscribe(x => {
        this.fundos = x;
      });
    });
  }
}
