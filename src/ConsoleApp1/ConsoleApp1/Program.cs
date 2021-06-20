using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ConsoleApp1
{
    public static class Extensions
    {
        public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, int, TAccumulate> func)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));

            int index = 0;
            TAccumulate result = seed;

            foreach (TSource element in source)
            {
                result = func(result, element, index);

                index++;
            }

            return result;
        }

        public static TResult Aggregate<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, int, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            int index = 0;
            TAccumulate result = seed;

            foreach (TSource element in source)
            {
                result = func(result, element, index);

                index++;
            }

            return resultSelector(result);
        }
    }

    public class AggregateBenchmark
    {
        private List<decimal> numbers;

        [GlobalSetup]
        public void Setup()
        {
            this.numbers = new List<decimal> { 235.5m, 238m, 233m, 235m, 232m, 240m, 244.5m, 246m, 240m, 236m, 233m, 235.5m, 244m, 245m, 248.5m, 241m, 238.5m, 234.5m, 240.5m, 239m, 248.5m, 249m, 242.5m, 244m, 243m, 242.5m, 242m, 247m, 250m, 250.5m, 254.5m, 256m, 252m, 254m, 259m, 264m, 264m, 265m, 265m, 261m, 261m, 260m, 259.5m, 256.5m, 251.5m, 246.5m, 248.5m, 248m, 253.5m, 251m, 246.5m, 249.5m, 248m, 250m, 252m, 254.5m, 254.5m, 254m, 254m, 248.5m, 250m, 252m, 254m, 259m, 257.5m, 254m, 257.5m, 263m, 263.5m, 265m, 261.5m, 263m, 262.5m, 265.5m, 265m, 267m, 265m, 264m, 264m, 265m, 266m, 268m, 272m, 280m, 279.5m, 276.5m, 276.5m, 278m, 286.5m, 282m, 290m, 293.5m, 296.5m, 293.5m, 293m, 290m, 294m, 293m, 293m, 293.5m, 294.5m, 298.5m, 299.5m, 298.5m, 299m, 307m, 310.5m, 311m, 309m, 305.5m, 301m, 305m, 304m, 303.5m, 307m, 311m, 315m, 313.5m, 311m, 309m, 307m, 307m, 311m, 309.5m, 305m, 307.5m, 307m, 306m, 312m, 313m, 316m, 313.5m, 319m, 331.5m, 339m, 336m, 345m, 344.5m, 335m, 329m, 334m, 332m, 333m, 333m, 338m, 334.5m, 331m, 339m, 339.5m, 332m, 329.5m, 329.5m, 337.5m, 339.5m, 341.5m, 346m, 340m, 334.5m, 333m, 333m, 316.5m, 320m, 315m, 325m, 327.5m, 332.5m, 328m, 327.5m, 331.5m, 335m, 335m, 335m, 331.5m, 322m, 326.5m, 325.5m, 325m, 320m, 322m, 318.5m, 316m, 311m, 317.5m, 320.5m, 323m, 315m, 305.5m, 307m, 302m, 294m, 290m, 276.5m, 268m, 260m, 248m, 270m, 255m, 267.5m, 277m, 280m, 273m, 267.5m, 274m, 271.5m, 275.5m, 283m, 285m, 283m, 279.5m, 278.5m, 285m, 287.5m, 286.5m, 306.5m, 304m, 295m, 294m, 295.5m, 294m, 298m, 296.5m, 299m, 304.5m, 295m, 295.5m, 296m, 297.5m, 297.5m, 301m, 295m, 297m, 293m, 298m, 290m, 291.5m, 294m, 297.5m, 292m, 292m, 295.5m, 296.5m, 294m, 292m, 295.5m, 296.5m, 301m, 306m, 311.5m, 318m, 319m, 322.5m, 320.5m, 316m, 309.5m, 315m, 315m, 314.5m, 314.5m, 312m, 315m, 317.5m, 312m, 313m, 317.5m, 322m, 329.5m, 338m, 338.5m, 341m, 345m, 348.5m, 354.5m, 363.5m, 363m, 357.5m, 367m, 366m, 383m, 384m, 381.5m, 386m, 424.5m, 435m, 422m, 434m, 425.5m, 416m, 425.5m, 429m, 435m, 433m, 435.5m, 429m, 419m, 429m, 427m, 435m, 433m, 427.5m, 415m, 424.5m, 428m, 434.5m, 442m, 444m, 435m, 426.5m, 435m, 433m, 436m, 429m, 426m, 431m, 427m, 435m, 436.5m, 441m, 445m, 458m, 448.5m, 444m, 440m, 437m, 433.5m, 423m, 424m, 431.5m, 431m, 433m, 432.5m, 439.5m, 443m, 453m, 460m, 462m, 459m, 453m, 449m, 457.5m, 451m, 453m, 455m, 452m, 450m, 447m, 444m, 437m, 432m, 435.5m, 441m, 450m, 451m, 452.5m, 458.5m, 451m, 457m, 458m, 462m, 484m, 485.5m, 497m, 490m, 488m, 496.5m, 492m, 487m, 489m, 489m, 480.5m, 490m, 499m, 497m, 503m, 514m, 524m, 520m, 512m, 516m, 508m, 504m, 512m, 508m, 510m, 516m, 509m, 509m, 510m, 511m, 515m, 515m, 525m, 530m, 536m, 542m, 549m, 565m, 580m, 584m, 591m, 605m, 592m, 601m, 607m, 627m, 647m, 673m, 649m, 633m, 617m, 615m, 601m, 591m, 611m, 632m, 630m, 627m, 632m, 663m, 660m, 652m, 650m, 641m, 625m, 635m, 606m, 609m, 622m, 601m, 601m, 598m, 595m, 597m, 609m, 614m, 611m, 613m, 604m, 602m, 591m, 593m, 594m, 576m, 575m, 590m, 599m, 597m, 587m, 602m, 610m, 610m, 613m, 610m, 605m, 605m, 612m, 619m, 610m, 603m, 602m, 592m, 591m, 602m, 610m, 610m, 602m, 600m, 588m, 591m, 585m, 587m, 599m, 589m, 571m, 560m, 547m, 557m, 549m, 572m, 567m, 567m, 573m, 568m, 583m, 585m, 582m, 590m, 597m, 598m, 595m, 596m, 595m, 592m, 589m, 586m, 599m, 602m, 609m, 605m, 606m, 603m };
        }

        [Benchmark]
        public void UsingForLoop()
        {
            var result = new List<decimal>();

            for (var i = 0; i < this.numbers.Count; i++)
            {
                if (i < 4) continue;

                if (this.numbers[i] > this.numbers.Skip(i - 4).Take(5).Average(n => n))
                {
                    result.Add(this.numbers[i]);
                }
            }
        }

        [Benchmark]
        public void UsingAggregate()
        {
            var result = this.numbers.Aggregate(
                    new { Index = default(int), Result = new List<decimal>() },
                    (accu, next) =>
                        {
                            if (accu.Index >= 4 && next > this.numbers.Skip(accu.Index - 4).Take(5).Average(n => n))
                            {
                                accu.Result.Add(next);
                            }

                            return new { Index = accu.Index + 1, accu.Result };
                        })
                .Result;
        }

        [Benchmark]
        public void UsingImprovedAggregate()
        {
            var result = this.numbers.Aggregate(
                new List<decimal>(),
                (accu, next, index) =>
                    {
                        if (index < 4) return accu;

                        if (next > this.numbers.Skip(index - 4).Take(5).Average(n => n))
                        {
                            accu.Add(next);
                        }

                        return accu;
                    });
        }

        [Benchmark]
        public void UsingThirdAggregate()
        {
            var result = this.numbers.Aggregate(
                new { Index = default(int), Result = new List<decimal>() },
                (accu, next) =>
                    {
                        if (accu.Index >= 4 && next > this.numbers.Skip(accu.Index - 4).Take(5).Average(n => n))
                        {
                            accu.Result.Add(next);
                        }

                        return new { Index = accu.Index + 1, accu.Result };
                    },
                accu =>
                    {
                        return string.Join(",", accu.Result);
                    });
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<AggregateBenchmark>();
        }
    }
}
