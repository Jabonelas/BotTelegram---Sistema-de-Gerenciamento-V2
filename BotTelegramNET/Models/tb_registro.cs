﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BotTelegramNET.Models
{
    public partial class tb_registro
    {
        public tb_registro()
        {
            tb_despesa = new HashSet<tb_despesa>();
            tb_nota_fiscal_saida = new HashSet<tb_nota_fiscal_saida>();
            tb_produto = new HashSet<tb_produto>();
        }

        [Key]
        public int id_registro { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_tipo_cadastro { get; set; }
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string rg_categoria { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_cpf { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_nome { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_sexo { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_rg { get; set; }
        [Column(TypeName = "date")]
        public DateTime? rg_data_nascimento { get; set; }
        [Column(TypeName = "date")]
        public DateTime rg_data_cadastro { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_observacoes { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_login { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_senha { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_cnpj { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_nome_fantasia { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_razao_social { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_email_xml { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_inscricao_estadual { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_inscricao_municipal { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_email { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_celular { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string rg_telefone_fixo { get; set; }
        public int? fk_permissoes { get; set; }
        public int? fk_informacao_comercial { get; set; }
        public int fk_endereco { get; set; }

        [ForeignKey("fk_endereco")]
        [InverseProperty("tb_registro")]
        public virtual tb_enderecos fk_enderecoNavigation { get; set; }
        [ForeignKey("fk_informacao_comercial")]
        [InverseProperty("tb_registro")]
        public virtual tb_informacoes_comerciais fk_informacao_comercialNavigation { get; set; }
        [ForeignKey("fk_permissoes")]
        [InverseProperty("tb_registro")]
        public virtual tb_permissoes fk_permissoesNavigation { get; set; }
        [InverseProperty("fk_registroNavigation")]
        public virtual ICollection<tb_despesa> tb_despesa { get; set; }
        [InverseProperty("fk_registro_clienteNavigation")]
        public virtual ICollection<tb_nota_fiscal_saida> tb_nota_fiscal_saida { get; set; }
        [InverseProperty("fk_registro_forncedorNavigation")]
        public virtual ICollection<tb_produto> tb_produto { get; set; }
    }
}