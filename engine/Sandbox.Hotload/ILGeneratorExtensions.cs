using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using Mono.Cecil;
using Mono.Cecil.Cil;

using EmitOpCodes = System.Reflection.Emit.OpCodes;
using EmitOpCode = System.Reflection.Emit.OpCode;
using CecilOpCodes = Mono.Cecil.Cil.OpCodes;
using CecilOpCode = Mono.Cecil.Cil.OpCode;

namespace Sandbox
{
	internal static class ILGeneratorExtensions
	{
		private static readonly EmitOpCode[] ConversionTable;
		static ILGeneratorExtensions()
		{
			var emitOpCodes = new List<EmitOpCode>();
			var max = 0;
			foreach ( var field in typeof( EmitOpCodes ).GetFields( BindingFlags.Public | BindingFlags.Static ) )
			{
				var value = (EmitOpCode)field.GetValue( null );
				emitOpCodes.Add( value );
				max = Math.Max( max, value.Value + 1 );
			}

			ConversionTable = new EmitOpCode[max];
			foreach ( var emitOpCode in emitOpCodes )
			{
				if ( emitOpCode.Value >= 0 ) ConversionTable[emitOpCode.Value] = emitOpCode;
			}
		}

		private static EmitOpCode Convert( this CecilOpCode code )
		{
			return ConversionTable[code.Value];
		}

		private static void MarkLabel( this ILGenerator il, Dictionary<Instruction, Label> labelDict, Instruction inst )
		{
			Label label;
			if ( !labelDict.TryGetValue( inst, out label ) ) return;

			il.MarkLabel( label );
		}

		private static Label DefineLabel( this ILGenerator il, Dictionary<Instruction, Label> labelDict, Instruction inst )
		{
			Label label;
			if ( labelDict.TryGetValue( inst, out label ) ) return label;

			label = il.DefineLabel();
			labelDict.Add( inst, label );

			return label;
		}

		private static LocalBuilder DeclareLocal( this ILGenerator il, Module context, Dictionary<VariableDefinition, LocalBuilder> localDict, VariableDefinition variable )
		{
			LocalBuilder local;
			if ( localDict.TryGetValue( variable, out local ) ) return local;

			var type = context.ResolveType( variable.VariableType );

			local = il.DeclareLocal( type, variable.IsPinned );
			localDict.Add( variable, local );

			return local;
		}

		public static void Emit( this ILGenerator il, Module context, Instruction first, Instruction until )
		{
			var list = new List<Instruction>();
			var labelDict = new Dictionary<Instruction, Label>();
			var localDict = new Dictionary<VariableDefinition, LocalBuilder>();

			do
			{
				list.Add( first );
			} while ( (first = first.Next) != until );

			foreach ( var inst in list )
			{
				if ( inst.Operand is Instruction label )
				{
					inst.Operand = il.DefineLabel( labelDict, label );
					continue;
				}

				if ( inst.Operand is Instruction[] labels )
				{
					var defined = new Label[labels.Length];

					for ( var i = 0; i < labels.Length; ++i )
					{
						defined[i] = il.DefineLabel( labelDict, labels[i] );
					}

					inst.Operand = defined;
					continue;
				}

				if ( inst.Operand is VariableDefinition vari )
				{
					inst.Operand = il.DeclareLocal( context, localDict, vari );
					continue;
				}

				if ( inst.Operand is TypeReference tr )
				{
					inst.Operand = context.ResolveType( tr );
					continue;
				}

				if ( inst.Operand is FieldReference fr )
				{
					inst.Operand = context.ResolveField( fr );
					continue;
				}

				if ( inst.Operand is MethodReference mr )
				{
					inst.Operand = context.ResolveMethod( mr );
					continue;
				}
			}

			foreach ( var inst in list )
			{
				il.MarkLabel( labelDict, inst );
				il.Emit( inst );
			}
		}

		private static void Emit( this ILGenerator il, Instruction inst )
		{
			var opCode = inst.OpCode.Convert();

			if ( inst.Operand == null )
			{
				il.Emit( opCode );
				return;
			}

			if ( inst.Operand is string )
			{
				il.Emit( opCode, (string)inst.Operand );
				return;
			}

			if ( inst.Operand is Label )
			{
				il.Emit( opCode, (Label)inst.Operand );
				return;
			}

			if ( inst.Operand is Label[] )
			{
				il.Emit( opCode, (Label[])inst.Operand );
				return;
			}

			if ( inst.Operand is Type )
			{
				il.Emit( opCode, (Type)inst.Operand );
				return;
			}

			if ( inst.Operand is FieldInfo )
			{
				il.Emit( opCode, (FieldInfo)inst.Operand );
				return;
			}

			if ( inst.Operand is MethodInfo )
			{
				il.Emit( opCode, (MethodInfo)inst.Operand );
				return;
			}

			if ( inst.Operand is ConstructorInfo )
			{
				il.Emit( opCode, (ConstructorInfo)inst.Operand );
				return;
			}

			if ( inst.Operand is LocalBuilder )
			{
				il.Emit( opCode, (LocalBuilder)inst.Operand );
				return;
			}

			if ( inst.Operand is sbyte )
			{
				il.Emit( opCode, (sbyte)inst.Operand );
				return;
			}

			if ( inst.Operand is byte )
			{
				il.Emit( opCode, (byte)inst.Operand );
				return;
			}

			if ( inst.Operand is short )
			{
				il.Emit( opCode, (short)inst.Operand );
				return;
			}

			if ( inst.Operand is int )
			{
				il.Emit( opCode, (int)inst.Operand );
				return;
			}

			if ( inst.Operand is long )
			{
				il.Emit( opCode, (long)inst.Operand );
				return;
			}

			if ( inst.Operand is float )
			{
				il.Emit( opCode, (float)inst.Operand );
				return;
			}

			if ( inst.Operand is double )
			{
				il.Emit( opCode, (double)inst.Operand );
				return;
			}

			throw new NotImplementedException( inst.Operand.GetType().FullName );
		}
	}
}
