using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Person;
using System.Text;

namespace FindingFightFraud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FraudFindingController : ControllerBase
    {
        [HttpPost]
        public IActionResult Compare(ArrayList person)
        {
            var p1 = Newtonsoft.Json.JsonConvert.DeserializeObject<Persons>(person[0].ToString());
            var p2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Persons>(person[1].ToString());
            int flag , total = 0;
            StringBuilder sb = new StringBuilder();
            if (p1.Identification.Equals(p2.Identification, StringComparison.InvariantCultureIgnoreCase))
                return Ok("Identification Matched 100 %");

            if (p1.LastName.Equals(p2.LastName, StringComparison.InvariantCultureIgnoreCase))
            {
                sb.AppendLine($"Last Name Matched {flag = 40 }% out of 40%;");
                total += flag;
            }

            flag = CompareSimiliarName(p1.FirstName.ToLower(), p2.FirstName.ToLower(), ref total);
            sb.AppendLine($"First Name Matched {flag }% out of 20%;");

            flag = CompareDOB(p1.Dob, p2.Dob, ref total);
            sb.AppendLine($"DOB Matched {flag }% out of 40%;");
            sb.AppendLine($"Toal Percentage is {total} %");
            return Ok(sb.ToString());
        }

        private int CompareSimiliarName(string name1, string name2, ref int total)
        {
            //Andrew and A. (initials)
            //- Andrew and Andew(typo)
            //-Andrew and Andy(diminutive)
            int flag = 0;

            if (name1.Equals(name2))
            {
                total += flag=20;
                return flag;
            }

            if (name1[0] == name2[0] && name2[1] == '.' && name2.Length == 2)
                flag = 15;
            else if (name1.Length >= 6 && (name2.Length >= 3))
            {
                //if first half alphabets of name1 matches to second name2
                //minimum lenght should be 3 or more
                string partialName1 = name1.Substring(0, name1.Length / 2);
                string partialName2 = name2.Substring(0, partialName1.Length);
                
                flag = (partialName1.ToLower().CompareTo(partialName2.ToLower()) == 0) ? 15 : 0;
            }
            total += flag;
            return flag;
        }

        private int CompareDOB(DateTime dob1, DateTime dob2, ref int total)
        {
            int flag;
            if (dob1 == dob2)
                flag=40;
            else
                 flag=0;
            total += flag;
            return flag;

        }

    }
}
