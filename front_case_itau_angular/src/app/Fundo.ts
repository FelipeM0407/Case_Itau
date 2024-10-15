export class Fundo {
    codigo: string;
  
    nome: string;
  
    cnpj: string;
  
    codigo_Tipo: number;
  
    patrimonio?: number;
  
    constructor(codigo: string, nome: string, cnpj: string, codigo_Tipo: number, patrimonio?: number) {
      this.codigo = codigo;
      this.nome = nome;
      this.cnpj = cnpj;
      this.codigo_Tipo = codigo_Tipo;
      this.patrimonio = patrimonio;
    }
  }
  