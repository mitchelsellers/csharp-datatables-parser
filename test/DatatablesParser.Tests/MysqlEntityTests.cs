using System;
using Xunit;
using DataTablesParser;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace DataTablesParser.Tests
{
  
    public class MysqlEntityTests
    {

        [Fact]
        public void TotalRecordsTest()
        {
            var context = TestHelper.GetMysqlContext();

            var p = TestHelper.CreateParams();

            var parser = new Parser<Person>(p, context.People);

            Console.WriteLine("Mysql - Total People TotalRecordsTest: {0}",context.People.Count());

            Assert.Equal(context.People.Count(),parser.Parse().recordsTotal);

        }

        [Fact]
        public void TotalResultsTest()
        {
            var context = TestHelper.GetMysqlContext();

            var p = TestHelper.CreateParams();

            var resultLength = 3;

            //override display length
            p[Constants.DISPLAY_LENGTH] = new StringValues(Convert.ToString(resultLength)); 

            var parser = new Parser<Person>(p, context.People);

            Console.WriteLine("Mysql - Total People TotalResultsTest: {0}",context.People.Count());

            Assert.Equal(resultLength, parser.Parse().data.Count);

        }

        [Fact]
        public void TotalDisplayTest()
        {
            var context = TestHelper.GetMysqlContext();
            var p = TestHelper.CreateParams();
            var displayLength = 1;

           
            //Set filter parameter
            p[Constants.SEARCH_KEY] = new StringValues("Cromie");

            var parser = new Parser<Person>(p, context.People);

            Console.WriteLine("Mysql - Total People TotalDisplayTest: {0}",context.People.Count());

            Assert.Equal(displayLength, parser.Parse().recordsFiltered);

        }


        [Fact]
        public void TotalDisplayIndividualTest()
        {
            var context = TestHelper.GetMysqlContext();
            var p = TestHelper.CreateParams();
            var displayLength = 1;

           
            //Set filter parameter
            p[Constants.SEARCH_KEY] = new StringValues("a");
            p[Constants.GetKey(Constants.SEARCH_VALUE_PROPERTY_FORMAT, "1")] = "mmer";

            var parser = new Parser<Person>(p, context.People);

            Console.WriteLine("MySql - Total People TotalDisplayIndividualTest: {0}",context.People.Count());

            Assert.Equal(displayLength, parser.Parse().recordsFiltered);

        }

        [Fact]
        public void TotalDisplayIndividualMutiTest()
        {
            var context = TestHelper.GetMysqlContext();
            var p = TestHelper.CreateParams();
            var displayLength = 1;

           
            //Set filter parameter
            p[Constants.SEARCH_KEY] = new StringValues("a");
            p[Constants.GetKey(Constants.SEARCH_VALUE_PROPERTY_FORMAT, "0")] = "omie";
            p[Constants.GetKey(Constants.SEARCH_VALUE_PROPERTY_FORMAT, "1")] = "mmer";
            
            var parser = new Parser<Person>(p, context.People);

            Console.WriteLine("MySql - Total People TotalDisplayIndividualMutiTest: {0}",context.People.Count());

            Assert.Equal(displayLength, parser.Parse().recordsFiltered);

        }

        [Fact]
        public void ResultsWhenSearchInNullColumnTest()
        {
            var context = TestHelper.GetMysqlContext();
            var p = TestHelper.CreateParams();
            var displayLength = 1;


            //Set filter parameter
            p[Constants.SEARCH_KEY] = new StringValues("Xorie");

            var parser = new Parser<Person>(p, context.People);

            var result = parser.Parse().recordsFiltered;

            Console.WriteLine("MySql - Search one row whe some columns are null: {0}", result);

            Assert.Equal(displayLength, result);

        }

        [Fact]
        public void AddCustomFilterTest()
        {
            var context = TestHelper.GetMysqlContext();
            var p = TestHelper.CreateParams();
            var displayLength = 2; // James and Tony


            var parser = new Parser<Person>(p, context.People); // p is empty, all rows

            var minDate = DateTime.Parse("1960-01-01");
            var maxDate = DateTime.Parse("1970-01-01");
            parser.AddCustomFilter(x => x.BirthDate >= minDate);
            parser.AddCustomFilter(x => x.BirthDate < maxDate);

            var result = parser.Parse().recordsFiltered;

            Console.WriteLine("MySql - Search only born between 1960 and 1970: {0}", result);

            Assert.Equal(displayLength, result);

        }

    }
}