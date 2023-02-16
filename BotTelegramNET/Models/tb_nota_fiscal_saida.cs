﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BotTelegramNET.Models
{
    public partial class tb_nota_fiscal_saida
    {
        [Key]
        public int id_nota_fiscal_saida { get; set; }
        public int nfs_numero_nf_saida { get; set; }
        [Column(TypeName = "date")]
        public DateTime nfs_data_emissao { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal nfs_quantidade { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal nfs_valor_parcial { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal nfs_valor_pago { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? nfs_valor_juros { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? nfs_valor_total_pago { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? nfs_valor_desconto { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string nfs_vendedor { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string nfs_tipo_pagamento { get; set; }
        public int? fk_estoque { get; set; }
        public int? fk_registro_cliente { get; set; }

        [ForeignKey("fk_estoque")]
        [InverseProperty("tb_nota_fiscal_saida")]
        public virtual tb_estoque fk_estoqueNavigation { get; set; }
        [ForeignKey("fk_registro_cliente")]
        [InverseProperty("tb_nota_fiscal_saida")]
        public virtual tb_registro fk_registro_clienteNavigation { get; set; }
    }
}