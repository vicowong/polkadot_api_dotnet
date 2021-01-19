﻿using System;
using System.Linq;
using System.Text;
using Polkadot.Api;
using Polkadot.BinaryContracts;
using Polkadot.DataStructs;
using Polkadot.Utils;
using Schnorrkel;
using Xunit;

namespace PolkaTest
{
    public class SignTests
    {
        [Fact]
        public void SignedMessageVerifies()
        {
            var message = new byte[] {0};
            var random = new Random();
            var message2 = new byte[1000];
            random.NextBytes(message2);

            using var application = (Application)PolkaApi.GetApplication();
            application.Connect(Constants.LocalNodeUri);
            var publicKey = application._protocolParams.Metadata.GetPublicKeyFromAddr(Constants.LocalAliceAddress).Bytes;
            var privateKey = Constants.LocalAlicePrivateKey;
            
            var sign = (ExtrinsicMultiSignature)application.Signer.Sign(publicKey, privateKey, message);
            var verify = application.Signer.VerifySignature(sign.MultiSignature.AsT1.Signature, publicKey, message);
            Assert.True(verify);

            var sign2 = (ExtrinsicMultiSignature)application.Signer.Sign(publicKey, privateKey, message2);
            var verify2 = application.Signer.VerifySignature(sign2.MultiSignature.AsT1.Signature, publicKey, message2);
            Assert.True(verify2);
        }
        
        [Theory]
        [InlineData(true, "0x00", "0xb0633663163e6149e1f1a429dbd606284d28bdb517c107d2384c2aadb6b95671089e97f2ccbbe2e4f15760b03b84e70e57845a61234b7382bdde0d8f653a908b")]
        [InlineData(true, "0xBA906E1D376304A5E828FB25E7F67034ACD8015A8575673C0FD6DA1EE38B4E3D44EC38921DB302BBE9D96B7A64CB8F36B7CD5B273B599B4429310E6188E5C25CDABEE0FBBDDE16F8D64570AB09EDD84346A46615302701EB50F369ECA4A03204170FA0A5EA9478D9B76250F5E9F03B3B58CA1AE47FAC2CB2BCF5AACA752A5435136C93FA4D3259B22B8D5ECED60C0260B82D93DA864D846BE2EDE9F5E9AE638C0E1AE17C5C41EE267168B9F24D5DDCAFF24A11489779AFC5E728175F4963C87C29D2C0BB2A8504763E6D681D94921D851039029F9C1BF5946B1C95342A8EDC0DA03CC87B9D20856D459B092A109130540A914F0F6028DF0DDD2B500EEFD04CAF905B7D57C9A2E8F0B494590111CA83D9EB4029983FCDAC775CBB9E71CF777D754E5A9AB514D88A67A95E5BCCCEDEFB12A87722AFA6B46FC2AE05FB0E0476213D16E4B3B730CDC513B5C1EEE172E0A97EBF8BC85855CECF9AEAD7070EDD7C63323D1DB5EA2A44E1DAB10FF9E4C58E50CD5C7C4E78AEC8E8481419CD59C6B735395EBBA3542BBF537BAAEEA645D5EAD45B873AB1612DEB8474356DD511768814FAD9BF8609EAFBA19C28D3CEC022A2722A4471D18B05D40B72372F1EBEEA410421D9AB6C1563CA718B9815385A1CC45293FAAACD05B688F505B414D2BBDCE2B2E6CEADE676365DC67A0046F2F09BB15673DC8BC43F74FF6A50B666614FF8B5AEB662518B6FDDDB5D9DE452057095056A02BE5FDE511E7371CF4296C0CFFB77814A973C8D4926E695012E265A21F9FABAB1FC5F9199D75D97EEE0BB2D471B8DA16CD1BAD8597720DBD7C9DD23F7184FD548DB0FD39CE5E8733AA59F696BB84EE5A084ED503F216139DB83B9CFF9399FD590E5E46B0CB90586E8479EB3D7B7701E7BF782722BFF96277AD5F8192C18790005CAD985D3E2D9E77F6767F882AD6124129A5401B72DAA904FBDF036C1EC5AD0CD9DAE0DDEEB4A981E9BF3C3979DA4CC73D30691C47849D81F483516F1A226930C972BE4FF5345274FBBE21C3AE6F9B11824C2EDE5C80B5454D6EE96B534A67C745FB7FF72BFCD208DDF4209952D9E394F84FDCDDF38AE438CD6902965AF711B1466A7DA2EF93F5857A25F068602BF2737B4473136D89C3CB174739C2FA3BADD6D27F42E85C30A5545A4763DD9A8BC8A1669F1B06840E43BCA6B29BA85A229B3E881985F0D90A174AAF1EC92625F6209B7E985C6B209752D42B8641A9A4FD0A27BC355580F5D7C85C77D06775451D96A0820039C1D6201DE0C6B1C552C86DA5B204A169F0A0AF95A6DFFFE3403FDD9F1DEF911794B348418C1764BA440265940A3AB56093A3A60FE26A92E1E64CCDA0F912B773375600FECC5FEDD93BE541946A6BD9158A6CFB6A4981353A20565B4F59C3C1214AF25CB439B", "0xac0fc4755bad193fe947fbf8747450502581065a6440e8703923f95dd5aa1125aa44c7c79e256e527fd39b68592e0114a41632fd3b9e71116064617f4d838987")]
        [InlineData(true, "0xFE709B330CCE8A0E82C886303ED8EC2EFCA09E236A4DBBC9A364809CBC44B2351ABF697091BC90831E35236124E196E54484B4B1350CD8B0B4D18FA84EED523D1551167E6E7B9D3F101A05352A59A5C2CE9A58379000FF0DE7B8BBBF2CE73531E2E54BD3CA1776C8E674A01B07A3865B2CCDDAB3951AAD14510897B6BFDE1228E5D452440DD1F7B6238F501B5FE405433260925F0D819BC7383681C27FC008086278353351424D5BD6239006CA91B56D0354835F4BB31A63388A0BD64F3536628648B8E7AED1F2EF133689EAF195FDA68114BE2B3CA9BCAF5DD2FBC636760BA1EC2D7F82D9CDBE3161944C142A3072A38C65543B760A25DD8B0989DCD62CB99C2AE8FDC3167BCDB4558DCAA85DA7852D5F5839D121D02AE207A1F6CF43E25A35DBAFEA4132AF8C1198A0E5E9407D80E3845C7D7EF2E87B5ABD1BBB7484FD70733294CDADBD43FAB0CB283880EB41A18A21134BE764DC42F2348F8417F3188C9A3071B6ABAFD2254061CCB3CAAFFA6E7B79305371B2880F5551DA792DBEE2BCB29C9E507A8AF5DA4EEE0A85982978433921089EC3C41BE7A7626059D559F7827F10002E10A9F4F083550583A803762CD9973F0CB644F9EE81B7A9182A9B05F4F6880687284A32902D171A6DBF973EA3EB33803E1AEACB3A1612AD5900DCC6E750118C20FCEF038E64AEDF9E2A8C2EE978052CF61BC7D90BBCCB71169DD8905150321DAD0B1EA6FC30F2D96B0B2C108AE3C3E1FAAFD0CC88378164317DEDF08B1E8E4DF76E80DE5A5E30C628ED6E008FBBE633D9F795A18066947689B9757E42014E4C7DDF191B62E5879C7F0AA49AC981D47597B9169489ECCD91D90A54A0E8B183269F2182B0A1CAD0F470F92276C1F9AD08E500E689EABAC3901188415890E6B3CD877DFBA7FE24D3CDE922248FC956CFE05C4BBBE8CBFA3CE45DF19E19887863894655EF44FE050AC02C199FC89634C7BBB861FB452BCA028C40174A0C86BA4470BE9A4973678AE15740FABCB15A744E292AE360267D1E5D7B536203E6BAFE3A71D52F09FBEA53532E7FA304310E5032CCC821E1A2591DF2A03DCE200486FA9C42C04ADB2A6A523ED78F32A6DA192C6D022F371E991C9718B73C68BD1C324FA78406C170C375086176E20A132776188711611DD54FDC0326283224ED029422A43F8A21D357BBA98D27D5A7B71183F14024E1ACBA157A58A7039B4890511A5FE1BF66D39C54442F56B790AD7A344C14A441E3C920E8688FC53279D0BC72183C0C75C6F7ABAF54EBDFB1560284CCDE73B787A34F6D513EF857D70D6ADB7473E39E54A1046E5468F5ECF7E96E92650AF953061834CFC9CEBB36CD9FD4A1EA106A3E7264302BDA04621EAE627668D066F2CC1416D45C3E14D586E6CDA0B4945302E67E301F00DACE79", "0x123e1611929fb45e2154436833dd950b2ad4aa37abcb0e3c757cabc952a78f2b3a194e624c87aefea8cc0b31069e7f33218df117c1abccb646ba57f2c5ec4583")]
        [InlineData(false, "0xBA906E1D376304A5E828FB25E7F67034ACD8015A8575673C0FD6DA1EE38B4E3D44EC38921DB302BBE9D96B7A64CB8F36B7CD5B273B599B4429310E6188E5C25CDABEE0FBBDDE16F8D64570AB09EDD84346A46615302701EB50F369ECA4A03204170FA0A5EA9478D9B76250F5E9F03B3B58CA1AE47FAC2CB2BCF5AACA752A5435136C93FA4D3259B22B8D5ECED60C0260B82D93DA864D846BE2EDE9F5E9AE638C0E1AE17C5C41EE267168B9F24D5DDCAFF24A11489779AFC5E728175F4963C87C29D2C0BB2A8504763E6D681D94921D851039029F9C1BF5946B1C95342A8EDC0DA03CC87B9D20856D459B092A109130540A914F0F6028DF0DDD2B500EEFD04CAF905B7D57C9A2E8F0B494590111CA83D9EB4029983FCDAC775CBB9E71CF777D754E5A9AB514D88A67A95E5BCCCEDEFB12A87722AFA6B46FC2AE05FB0E0476213D16E4B3B730CDC513B5C1EEE172E0A97EBF8BC85855CECF9AEAD7070EDD7C63323D1DB5EA2A44E1DAB10FF9E4C58E50CD5C7C4E78AEC8E8481419CD59C6B735395EBBA3542BBF537BAAEEA645D5EAD45B873AB1612DEB8474356DD511768814FAD9BF8609EAFBA19C28D3CEC022A2722A4471D18B05D40B72372F1EBEEA410421D9AB6C1563CA718B9815385A1CC45293FAAACD05B688F505B414D2BBDCE2B2E6CEADE676365DC67A0046F2F09BB15673DC8BC43F74FF6A50B666614FF8B5AEB662518B6FDDDB5D9DE452057095056A02BE5FDE511E7371CF4296C0CFFB77814A973C8D4926E695012E265A21F9FABAB1FC5F9199D75D97EEE0BB2D471B8DA16CD1BAD8597720DBD7C9DD23F7184FD548DB0FD39CE5E8733AA59F696BB84EE5A084ED503F216139DB83B9CFF9399FD590E5E46B0CB90586E8479EB3D7B7701E7BF782722BFF96277AD5F8192C18790005CAD985D3E2D9E77F6767F882AD6124129A5401B72DAA904FBDF036C1EC5AD0CD9DAE0DDEEB4A981E9BF3C3979DA4CC73D30691C47849D81F483516F1A226930C972BE4FF5345274FBBE21C3AE6F9B11824C2EDE5C80B5454D6EE96B534A67C745FB7FF72BFCD208DDF4209952D9E394F84FDCDDF38AE438CD6902965AF711B1466A7DA2EF93F5857A25F068602BF2737B4473136D89C3CB174739C2FA3BADD6D27F42E85C30A5545A4763DD9A8BC8A1669F1B06840E43BCA6B29BA85A229B3E881985F0D90A174AAF1EC92625F6209B7E985C6B209752D42B8641A9A4FD0A27BC355580F5D7C85C77D06775451D96A0820039C1D6201DE0C6B1C552C86DA5B204A169F0A0AF95A6DFFFE3403FDD9F1DEF911794B348418C1764BA440265940A3AB56093A3A60FE26A92E1E64CCDA0F912B773375600FECC5FEDD93BE541946A6BD9158A6CFB6A4981353A20565B4F59C3C1214AF25CB439B", "0xac0fc4755bad193fe947fbf8747450502581065a6440e8703923f95dd5aa1125aa44c7c79e256e527fd39b68592e0114a41632fd3b9e71116064617f4d838988")]
        [InlineData(false, "0xBA906E1D376304A5E828FB25E7F67034ACD8015A8575673C0FD6DA1EE38B4E3D44EC38921DB302BBE9D96B7A64CB8F36B7CD5B273B599B4429310E6188E5C25CDABEE0FBBDDE16F8D64570AB09EDD84346A46615302701EB50F369ECA4A03204170FA0A5EA9478D9B76250F5E9F03B3B58CA1AE47FAC2CB2BCF5AACA752A5435136C93FA4D3259B22B8D5ECED60C0260B82D93DA864D846BE2EDE9F5E9AE638C0E1AE17C5C41EE267168B9F24D5DDCAFF24A11489779AFC5E728175F4963C87C29D2C0BB2A8504763E6D681D94921D851039029F9C1BF5946B1C95342A8EDC0DA03CC87B9D20856D459B092A109130540A914F0F6028DF0DDD2B500EEFD04CAF905B7D57C9A2E8F0B494590111CA83D9EB4029983FCDAC775CBB9E71CF777D754E5A9AB514D88A67A95E5BCCCEDEFB12A87722AFA6B46FC2AE05FB0E0476213D16E4B3B730CDC513B5C1EEE172E0A97EBF8BC85855CECF9AEAD7070EDD7C63323D1DB5EA2A44E1DAB10FF9E4C58E50CD5C7C4E78AEC8E8481419CD59C6B735395EBBA3542BBF537BAAEEA645D5EAD45B873AB1612DEB8474356DD511768814FAD9BF8609EAFBA19C28D3CEC022A2722A4471D18B05D40B72372F1EBEEA410421D9AB6C1563CA718B9815385A1CC45293FAAACD05B688F505B414D2BBDCE2B2E6CEADE676365DC67A0046F2F09BB15673DC8BC43F74FF6A50B666614FF8B5AEB662518B6FDDDB5D9DE452057095056A02BE5FDE511E7371CF4296C0CFFB77814A973C8D4926E695012E265A21F9FABAB1FC5F9199D75D97EEE0BB2D471B8DA16CD1BAD8597720DBD7C9DD23F7184FD548DB0FD39CE5E8733AA59F696BB84EE5A084ED503F216139DB83B9CFF9399FD590E5E46B0CB90586E8479EB3D7B7701E7BF782722BFF96277AD5F8192C18790005CAD985D3E2D9E77F6767F882AD6124129A5401B72DAA904FBDF036C1EC5AD0CD9DAE0DDEEB4A981E9BF3C3979DA4CC73D30691C47849D81F483516F1A226930C972BE4FF5345274FBBE21C3AE6F9B11824C2EDE5C80B5454D6EE96B534A67C745FB7FF72BFCD208DDF4209952D9E394F84FDCDDF38AE438CD6902965AF711B1466A7DA2EF93F5857A25F068602BF2737B4473136D89C3CB174739C2FA3BADD6D27F42E85C30A5545A4763DD9A8BC8A1669F1B06840E43BCA6B29BA85A229B3E881985F0D90A174AAF1EC92625F6209B7E985C6B209752D42B8641A9A4FD0A27BC355580F5D7C85C77D06775451D96A0820039C1D6201DE0C6B1C552C86DA5B204A169F0A0AF95A6DFFFE3403FDD9F1DEF911794B348418C1764BA440265940A3AB56093A3A60FE26A92E1E64CCDA0F912B773375600FECC5FEDD93BE541946A6BD9158A6CFB6A4981353A20565B4F59C3C1214AF25CB438B", "0xac0fc4755bad193fe947fbf8747450502581065a6440e8703923f95dd5aa1125aa44c7c79e256e527fd39b68592e0114a41632fd3b9e71116064617f4d838987")]
        [InlineData(false, "0x001180CFC171BB038575FFF5F7405C2CAA9097BD148951C946367937726597AB32B0473419B3552073843F82DECD2E1CEC94865B816F0976BFB7888AF8849020DD54C346F82812F63A77498F49CFEA79075C4ADDDE3C29C2A6D174622BCA28EFF41FA48A09E723C38190200A267A2CEFCC0E829B39B241A02A009F27B3B0CB6F4E5A9B2A2A606F1F39875C4E1B1F9C2CA484951C478DB541DD04600947FB0FD0C5B0BBCBDF02C56C9F6262B256F3840A6473832E1BAE27C1A4594D260066D83D778B886B63D04DA1BF287CE42B03EAE97F55A4D478E0B51688F92685B59E6C398A243BCA6A19A69326E16FF2314296A16F63A7AAC103C496D672E269A9C7EC21BC435A06C71F7F65AE11AD9671C2E54563851651F7C177875FA4F4F8373CE95EF44D36EDE890D94FE441FCB0562E24148D9C0624B19B29FBCBD3A6232B862E72274EA2248BF75DD3ABC66DB6C5C7313180DF6A9A36C5AD59233A2115E9B6235D128988078B0088963F3378EA2D733604A175A24B1B933B62A1480051D547E9F5A310A33B3E29A57F57D45B024CC22BAF041D737414D8417F4D2665430037C0F74983471CD0FBC578CF22F447373916DE1C504CB7D375E0CACF24ED18C4627F1F1364B9C87B0FDF0857FBA5F1E3F94714108626AFEA0924D3A27C4DAC0EADB36CA2D433D37F4D630D70435FD8D0EFE6D5DCD8083D3CCF340A40E525FEC133FAAA47384E0FC67395C223183FA6A94C03B39577CF36A47630D29F28939A28654B3D49B49097A0D70F6843461A23908D61F4E271684963C406D1087B6F9DE0FB99BDCC015DF4BB6B12BE6866F35E5C0C4211C79FB8B6FD02D984AEBE8F2467718847AC5C578F306C38765EA9E1FDD1A774FB395DD64042E2C0CA27D845FCDBED56F7DDABB100B5B2E15A6741585321DCB3074A94EB099A4805CA9B8C6757273B9A7052E91F26F67445877C7BD9018A6DA94A253EB247024D5C95A6E764294A8A332CA6E70037BD8C10D54E638D1F6F033F2DE21C32E888536D058C8A800E0F1C3CF248AC5AA36C728D982DDDF99FC03D26A2A737462D584BA35379FBAA330C02C531279AD7C2A99CC0708F99BAAB5ACE454F63DE66D8D60E9AFD09062A71C75A8B2B17C777256663F80B76D59EBE90F8F9AAB65BCF2E9BED5A1900D9B8A2714FA9340D8F9492571FE800E55B61016FDB6C110D80BB6C8B5D90296D5E248CAA5699272C69B1006467529672CF493F8B59A9246DA795F1933DA9AFFF6A25F9FA5C558FC23AD5062A6BACA52D25F9BC14509E13B4001F02C0A794EEBF0101F942599B1579BA7D5184A3F901FBF0C46D43ED9BAFCC1145FA3D52CE1AA6E646296AB363D38AE3425AD24D8F11061D07D0E226ABD265522D3014A0D827EB62AA9D5DD096478CF0E35FAA41DC7F", "0xD0742E176DB9717B86F6D907FD341089FF93BC5B3D4EC618C4960D7BF9A03A3CE8C6220EEB9E7E198121276C406D018A1629B5222F7B787E75BE33BCBC591200")]
        public void VerificationOfWebSignSuccessful(bool isValid, string strMessage, string strSign)
        {
            using var application = (Application)PolkaApi.GetApplication();
            application.Connect(Constants.LocalNodeUri);

            var message = strMessage.HexToByteArray();

            var webSign = strSign.HexToByteArray();
            var alicePublicKey = application._protocolParams.Metadata.GetPublicKeyFromAddr(Constants.LocalAliceAddress).Bytes;
            try
            {
                var verify = application.Signer.VerifySignature(
                    webSign,
                    alicePublicKey,
                    message);
                Assert.Equal(isValid, verify);
            }
            catch (Exception)
            {
                if (!isValid)
                {
                    return;
                }

                throw;
            }

        }
    }
}