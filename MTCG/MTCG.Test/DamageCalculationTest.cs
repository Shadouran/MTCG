using NUnit.Framework;
using MTCG.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MTCG.DAL;
using MTCG.Models;

namespace MTCG.Test
{
    [TestFixture]
    class DamageCalculationTest
    {
        [Test]
        public void GoblinDamageVersusDragon()
        {
            var battleHandler = new BattleHandler();
            var goblin = new Card
            {
                Name = "Goblin",
                CardType = CardType.Goblin,
                Element = Element.Regular,
                Damage = 100
            };
            var dragon = new Card
            {
                Name = "Dragon",
                CardType = CardType.Dragon,
                Element = Element.Regular,
                Damage = 50
            };
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(goblin, dragon));
        }

        [Test]
        public void GoblinDamageVersusFireDragon()
        {
            var battleHandler = new BattleHandler();
            var goblin = new Card
            {
                Name = "Goblin",
                CardType = CardType.Goblin,
                Element = Element.Regular,
                Damage = 100
            };
            var dragon = new Card
            {
                Name = "FireDragon",
                CardType = CardType.Dragon,
                Element = Element.Fire,
                Damage = 50
            };
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(goblin, dragon));
        }

        [Test]
        public void GoblinDamageVersusWaterDragon()
        {
            var battleHandler = new BattleHandler();
            var goblin = new Card
            {
                Name = "Goblin",
                CardType = CardType.Goblin,
                Element = Element.Regular,
                Damage = 100
            };
            var dragon = new Card
            {
                Name = "WaterDragon",
                CardType = CardType.Dragon,
                Element = Element.Water,
                Damage = 50
            };
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(goblin, dragon));
        }

        [Test]
        public void DragonDamageVersusFireElf()
        {
            var battleHandler = new BattleHandler();
            var dragon = new Card
            {
                Name = "Dragon",
                CardType = CardType.Dragon,
                Element = Element.Regular,
                Damage = 100
            };
            var elf = new Card
            {
                Name = "FireElf",
                CardType = CardType.Elf,
                Element = Element.Fire,
                Damage = 50
            };
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(dragon, elf));
        }

        [Test]
        public void OrkDamageVersusWizzard()
        {
            var battleHandler = new BattleHandler();
            var ork = new Card
            {
                Name = "Ork",
                CardType = CardType.Ork,
                Element = Element.Regular,
                Damage = 100
            };
            var wizzard = new Card
            {
                Name = "Wizzard",
                CardType = CardType.Wizzard,
                Element = Element.Regular,
                Damage = 50
            };
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(ork, wizzard));
        }

        [Test]
        public void OrkDamageVersusFireWizzard()
        {
            var battleHandler = new BattleHandler();
            var ork = new Card
            {
                Name = "Ork",
                CardType = CardType.Ork,
                Element = Element.Regular,
                Damage = 100
            };
            var wizzard = new Card
            {
                Name = "FireWizzard",
                CardType = CardType.Wizzard,
                Element = Element.Fire,
                Damage = 50
            };
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(ork, wizzard));
        }

        [Test]
        public void OrkDamageVersusWaterWizzard()
        {
            var battleHandler = new BattleHandler();
            var ork = new Card
            {
                Name = "Ork",
                CardType = CardType.Ork,
                Element = Element.Regular,
                Damage = 100
            };
            var wizzard = new Card
            {
                Name = "WaterWizzard",
                CardType = CardType.Wizzard,
                Element = Element.Water,
                Damage = 50
            };
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(ork, wizzard));
        }

        [Test]
        public void KnightDamageVersusWaterSpell()
        {
            var battleHandler = new BattleHandler();
            var knight = new Card
            {
                Name = "Knight",
                CardType = CardType.Knight,
                Element = Element.Regular,
                Damage = 100
            };
            var spell = new Card
            {
                Name = "WaterSpell",
                CardType = CardType.Spell,
                Element = Element.Water,
                Damage = 50
            };
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(knight, spell));
        }

        [Test]
        public void FireKnightDamageVersusWaterSpell()
        {
            var battleHandler = new BattleHandler();
            var knight = new Card
            {
                Name = "FireKnight",
                CardType = CardType.Knight,
                Element = Element.Fire,
                Damage = 100
            };
            var spell = new Card
            {
                Name = "WaterSpell",
                CardType = CardType.Spell,
                Element = Element.Water,
                Damage = 50
            };
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(knight, spell));
        }

        [Test]
        public void RegularSpellDamageVersusAllElementsKraken()
        {
            var battleHandler = new BattleHandler();
            var spell = new Card
            {
                Name = "Spell",
                CardType = CardType.Spell,
                Element = Element.Regular,
                Damage = 100
            };
            var kraken = new Card
            {
                Name = "Kraken",
                CardType = CardType.Kraken,
                Element = Element.Regular,
                Damage = 50
            };
            var fireKraken = new Card
            {
                Name = "Kraken",
                CardType = CardType.Kraken,
                Element = Element.Fire,
                Damage = 50
            };
            var waterKraken = new Card
            {
                Name = "Kraken",
                CardType = CardType.Kraken,
                Element = Element.Water,
                Damage = 50
            };
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(spell, kraken));
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(spell, fireKraken));
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(spell, waterKraken));
        }

        [Test]
        public void FireSpellDamageVersusAllElementsKraken()
        {
            var battleHandler = new BattleHandler();
            var spell = new Card
            {
                Name = "FireSpell",
                CardType = CardType.Spell,
                Element = Element.Fire,
                Damage = 100
            };
            var kraken = new Card
            {
                Name = "Kraken",
                CardType = CardType.Kraken,
                Element = Element.Regular,
                Damage = 50
            };
            var fireKraken = new Card
            {
                Name = "Kraken",
                CardType = CardType.Kraken,
                Element = Element.Fire,
                Damage = 50
            };
            var waterKraken = new Card
            {
                Name = "Kraken",
                CardType = CardType.Kraken,
                Element = Element.Water,
                Damage = 50
            };
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(spell, kraken));
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(spell, fireKraken));
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(spell, waterKraken));
        }

        [Test]
        public void WaterSpellDamageVersusAllElementsKraken()
        {
            var battleHandler = new BattleHandler();
            var spell = new Card
            {
                Name = "WaterSpell",
                CardType = CardType.Spell,
                Element = Element.Water,
                Damage = 100
            };
            var kraken = new Card
            {
                Name = "Kraken",
                CardType = CardType.Kraken,
                Element = Element.Regular,
                Damage = 50
            };
            var fireKraken = new Card
            {
                Name = "Kraken",
                CardType = CardType.Kraken,
                Element = Element.Fire,
                Damage = 50
            };
            var waterKraken = new Card
            {
                Name = "Kraken",
                CardType = CardType.Kraken,
                Element = Element.Water,
                Damage = 50
            };
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(spell, kraken));
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(spell, fireKraken));
            Assert.AreEqual(0.0, battleHandler.DamageCalculation(spell, waterKraken));
        }

        [Test]
        public void DefaultMonsterDamageVersusMonsterAllElements()
        {
            var battleHandler = new BattleHandler();
            var goblin = new Card
            {
                Name = "Goblin",
                CardType = CardType.Goblin,
                Element = Element.Regular,
                Damage = 100
            };
            var fireGoblin = new Card
            {
                Name = "FireGoblin",
                CardType = CardType.Goblin,
                Element = Element.Fire,
                Damage = 100
            };
            var waterGoblin = new Card
            {
                Name = "WaterGoblin",
                CardType = CardType.Goblin,
                Element = Element.Water,
                Damage = 100
            };
            Assert.AreEqual(100, battleHandler.DamageCalculation(goblin, goblin));
            Assert.AreEqual(100, battleHandler.DamageCalculation(goblin, fireGoblin));
            Assert.AreEqual(100, battleHandler.DamageCalculation(goblin, waterGoblin));
            Assert.AreEqual(100, battleHandler.DamageCalculation(fireGoblin, goblin));
            Assert.AreEqual(100, battleHandler.DamageCalculation(fireGoblin, fireGoblin));
            Assert.AreEqual(100, battleHandler.DamageCalculation(fireGoblin, waterGoblin));
            Assert.AreEqual(100, battleHandler.DamageCalculation(waterGoblin, goblin));
            Assert.AreEqual(100, battleHandler.DamageCalculation(waterGoblin, fireGoblin));
            Assert.AreEqual(100, battleHandler.DamageCalculation(waterGoblin, waterGoblin));
        }

        [Test]
        public void DefaultSpellDamageVersusSpellAllElements()
        {
            var battleHandler = new BattleHandler();
            var spell = new Card
            {
                Name = "Spell",
                CardType = CardType.Spell,
                Element = Element.Regular,
                Damage = 100
            };
            var fireSpell = new Card
            {
                Name = "FireSpell",
                CardType = CardType.Spell,
                Element = Element.Fire,
                Damage = 100
            };
            var waterSpell = new Card
            {
                Name = "WaterSpell",
                CardType = CardType.Spell,
                Element = Element.Water,
                Damage = 100
            };

            Assert.AreEqual(100, battleHandler.DamageCalculation(spell, spell));
            Assert.AreEqual(50, battleHandler.DamageCalculation(spell, fireSpell));
            Assert.AreEqual(200, battleHandler.DamageCalculation(spell, waterSpell));

            Assert.AreEqual(200, battleHandler.DamageCalculation(fireSpell, spell));
            Assert.AreEqual(100, battleHandler.DamageCalculation(fireSpell, fireSpell));
            Assert.AreEqual(50, battleHandler.DamageCalculation(fireSpell, waterSpell));

            Assert.AreEqual(50, battleHandler.DamageCalculation(waterSpell, spell));
            Assert.AreEqual(200, battleHandler.DamageCalculation(waterSpell, fireSpell));
            Assert.AreEqual(100, battleHandler.DamageCalculation(waterSpell, waterSpell));
        }

        [Test]
        public void DefaultSpellDamageVersusAllElementsMonster()
        {
            var battleHandler = new BattleHandler();
            var spell = new Card
            {
                Name = "Spell",
                CardType = CardType.Spell,
                Element = Element.Regular,
                Damage = 100
            };
            var goblin = new Card
            {
                Name = "Goblin",
                CardType = CardType.Goblin,
                Element = Element.Regular,
                Damage = 100
            };
            var fireGoblin = new Card
            {
                Name = "FireGoblin",
                CardType = CardType.Goblin,
                Element = Element.Fire,
                Damage = 100
            };
            var waterGoblin = new Card
            {
                Name = "WaterGoblin",
                CardType = CardType.Goblin,
                Element = Element.Water,
                Damage = 100
            };

            Assert.AreEqual(100, battleHandler.DamageCalculation(spell, goblin));
            Assert.AreEqual(50, battleHandler.DamageCalculation(spell, fireGoblin));
            Assert.AreEqual(200, battleHandler.DamageCalculation(spell, waterGoblin));
        }

        [Test]
        public void DefaultFireSpellDamageVersusAllElementsMonster()
        {
            var battleHandler = new BattleHandler();
            var fireSpell = new Card
            {
                Name = "FireSpell",
                CardType = CardType.Spell,
                Element = Element.Fire,
                Damage = 100
            };
            var goblin = new Card
            {
                Name = "Goblin",
                CardType = CardType.Goblin,
                Element = Element.Regular,
                Damage = 100
            };
            var fireGoblin = new Card
            {
                Name = "FireGoblin",
                CardType = CardType.Goblin,
                Element = Element.Fire,
                Damage = 100
            };
            var waterGoblin = new Card
            {
                Name = "WaterGoblin",
                CardType = CardType.Goblin,
                Element = Element.Water,
                Damage = 100
            };

            Assert.AreEqual(200, battleHandler.DamageCalculation(fireSpell, goblin));
            Assert.AreEqual(100, battleHandler.DamageCalculation(fireSpell, fireGoblin));
            Assert.AreEqual(50, battleHandler.DamageCalculation(fireSpell, waterGoblin));
        }

        [Test]
        public void DefaultWaterSpellDamageVersusAllElementsMonster()
        {
            var battleHandler = new BattleHandler();
            var waterSpell = new Card
            {
                Name = "FireSpell",
                CardType = CardType.Spell,
                Element = Element.Water,
                Damage = 100
            };
            var goblin = new Card
            {
                Name = "Goblin",
                CardType = CardType.Goblin,
                Element = Element.Regular,
                Damage = 100
            };
            var fireGoblin = new Card
            {
                Name = "FireGoblin",
                CardType = CardType.Goblin,
                Element = Element.Fire,
                Damage = 100
            };
            var waterGoblin = new Card
            {
                Name = "WaterGoblin",
                CardType = CardType.Goblin,
                Element = Element.Water,
                Damage = 100
            };

            Assert.AreEqual(50, battleHandler.DamageCalculation(waterSpell, goblin));
            Assert.AreEqual(200, battleHandler.DamageCalculation(waterSpell, fireGoblin));
            Assert.AreEqual(100, battleHandler.DamageCalculation(waterSpell, waterGoblin));
        }

        [Test]
        public void DefaultMonsterDamageVersusAllElementsSpell()
        {
            var battleHandler = new BattleHandler();
            var goblin = new Card
            {
                Name = "Goblin",
                CardType = CardType.Goblin,
                Element = Element.Regular,
                Damage = 100
            };
            var spell = new Card
            {
                Name = "Spell",
                CardType = CardType.Spell,
                Element = Element.Regular,
                Damage = 100
            };
            var fireSpell = new Card
            {
                Name = "FireSpell",
                CardType = CardType.Spell,
                Element = Element.Fire,
                Damage = 100
            };
            var waterSpell = new Card
            {
                Name = "WaterSpell",
                CardType = CardType.Spell,
                Element = Element.Water,
                Damage = 100
            };

            Assert.AreEqual(100, battleHandler.DamageCalculation(goblin, spell));
            Assert.AreEqual(50, battleHandler.DamageCalculation(goblin, fireSpell));
            Assert.AreEqual(200, battleHandler.DamageCalculation(goblin, waterSpell));
        }

        [Test]
        public void DefaultFireMonsterDamageVersusAllElementsSpell()
        {
            var battleHandler = new BattleHandler();
            var fireGoblin = new Card
            {
                Name = "FireGoblin",
                CardType = CardType.Goblin,
                Element = Element.Fire,
                Damage = 100
            };
            var spell = new Card
            {
                Name = "Spell",
                CardType = CardType.Spell,
                Element = Element.Regular,
                Damage = 100
            };
            var fireSpell = new Card
            {
                Name = "FireSpell",
                CardType = CardType.Spell,
                Element = Element.Fire,
                Damage = 100
            };
            var waterSpell = new Card
            {
                Name = "WaterSpell",
                CardType = CardType.Spell,
                Element = Element.Water,
                Damage = 100
            };

            Assert.AreEqual(200, battleHandler.DamageCalculation(fireGoblin, spell));
            Assert.AreEqual(100, battleHandler.DamageCalculation(fireGoblin, fireSpell));
            Assert.AreEqual(50, battleHandler.DamageCalculation(fireGoblin, waterSpell));
        }

        [Test]
        public void DefaultWaterMonsterDamageVersusAllElementsSpell()
        {
            var battleHandler = new BattleHandler();
            var waterGoblin = new Card
            {
                Name = "WaterGoblin",
                CardType = CardType.Goblin,
                Element = Element.Water,
                Damage = 100
            };
            var spell = new Card
            {
                Name = "Spell",
                CardType = CardType.Spell,
                Element = Element.Regular,
                Damage = 100
            };
            var fireSpell = new Card
            {
                Name = "FireSpell",
                CardType = CardType.Spell,
                Element = Element.Fire,
                Damage = 100
            };
            var waterSpell = new Card
            {
                Name = "WaterSpell",
                CardType = CardType.Spell,
                Element = Element.Water,
                Damage = 100
            };

            Assert.AreEqual(50, battleHandler.DamageCalculation(waterGoblin, spell));
            Assert.AreEqual(200, battleHandler.DamageCalculation(waterGoblin, fireSpell));
            Assert.AreEqual(100, battleHandler.DamageCalculation(waterGoblin, waterSpell));
        }
    }
}
