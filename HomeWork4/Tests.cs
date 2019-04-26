using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace HomeWork4
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void ShouldAddItemsOnCommit()
        {
            TransactionalHashTable<int, string> transactHashTable = new TransactionalHashTable<int, string>();
            List<(int key, string value)> itemsToAdd = new List<(int key, string value)>()
                { (1, "lorem"), (2, "ipsum"), (3, "dolor ") };

            using (Transaction<int, string> transaction = transactHashTable.BeginTransaction())
            {
                foreach (var (key, value) in itemsToAdd)
                {
                    transaction.Add(key, value);
                }

                Assert.True(transactHashTable.Count == 0); // Items are not in hashTable, until they are commited.
            }

            Assert.True(transactHashTable.Count == itemsToAdd.Count);
        }

        [Test]
        public void ShouldNotAddItemsOnRollback()
        {
            TransactionalHashTable<int, string> transactHashTable = new TransactionalHashTable<int, string>();
            List<(int key, string value)> itemsToAdd = new List<(int key, string value)>()
                { (1, "lorem"), (2, "ipsum"), (3, "dolor ") };

            Transaction<int, string> transaction = transactHashTable.BeginTransaction();
            foreach (var (key, value) in itemsToAdd)
            {
                transaction.Add(key, value);
            }
            Assert.False(transactHashTable.Count == itemsToAdd.Count);

            transactHashTable.RollbackTransation(transaction);
            // transaction.RollBack(); // Can also be used for the same result.

            Assert.True(transaction.ChangesCount == 0); // Transaction is reset.
        }

        [Test]
        public void ShouldRemoveItemsOnCommit()
        {
            TransactionalHashTable<int, string> transactHashTable = new TransactionalHashTable<int, string>(
                (1, "lorem"), (2, "ipsum"), (3, "dolor "));
            List<(int key, string value)> itemsToRemove = new List<(int key, string value)>()
                { (2, "ipsum"), (3, "dolor ") };

            using (Transaction<int, string> transaction = transactHashTable.BeginTransaction())
            {
                foreach (var (key, value) in itemsToRemove)
                {
                    transaction.Remove(key);
                }

                Assert.True(transactHashTable.Count == 3);
            }

            Assert.True(transactHashTable.Count == 1);
        }

        [Test]
        public void ShoudNotRemoveItemsOnRollback()
        {
            TransactionalHashTable<int, string> transactHashTable = new TransactionalHashTable<int, string>(
                (1, "lorem"), (2, "ipsum"), (3, "dolor "));
            List<(int key, string value)> itemsToRemove = new List<(int key, string value)>()
                { (2, "ipsum"), (3, "dolor ") };

            Transaction<int, string> transaction = transactHashTable.BeginTransaction();
            foreach (var (key, value) in itemsToRemove)
            {
                transaction.Remove(key);
            }
            Assert.True(transactHashTable.Count == 3);
            Assert.True(transaction.ChangesCount == 2);

            transactHashTable.RollbackTransation(transaction);
            Assert.True(transactHashTable.Count == 3);
            Assert.True(transaction.ChangesCount == 0);
        }

        [Test]
        public void ShouldThrowIfFinished()
        {
            TransactionalHashTable<int, string> transactHashTable = new TransactionalHashTable<int, string>();
            Transaction<int, string> transaction = transactHashTable.BeginTransaction();
            transactHashTable.Commit(transaction); // The transaction is finished now.
            //transaction.Execute(); // Can also be used for the same result.
            //transaction.Dispose(); // Can also be used for the same result.

            Assert.Catch<InvalidOperationException>(() => transactHashTable.Commit(transaction));
            Assert.Catch<InvalidOperationException>(() => transactHashTable.RollbackTransation(transaction));
            Assert.Catch<InvalidOperationException>(() => transaction.Add(1, "hello"));
            Assert.Catch<InvalidOperationException>(() => transaction.Get(1));
            Assert.Catch<InvalidOperationException>(() => transaction.TryGet(1, out _));
            Assert.Catch<InvalidOperationException>(() => transaction.Remove(1));
        }

        [Test]
        public void ShouldHandleSubsequentTransactions()
        {
            TransactionalHashTable<int, string> transactHashTable = new TransactionalHashTable<int, string>(
                (1, "lorem"), (2, "ipsum"), (3, "dolor "));

            using (Transaction<int, string> firstTransaction = transactHashTable.BeginTransaction())
            {
                using (Transaction<int, string> secondTransaction = transactHashTable.BeginTransaction())
                {
                    secondTransaction.Remove(2);
                    secondTransaction.Remove(3);
                }
                Assert.True(transactHashTable.Count == 1);

                firstTransaction.Add(2, "ipsum");
                firstTransaction.Add(3, "dolor ");
            }
            Assert.True(transactHashTable.Count == 3);
        }

        [Test]
        public void ShouldReserChangesWhenOpposite()
        {
            TransactionalHashTable<int, string> transactHashTable = new TransactionalHashTable<int, string>(
                (1, "lorem"), (2, "ipsum"), (3, "dolor "));

            using (Transaction<int, string> transaction = transactHashTable.BeginTransaction())
            {
                transaction.Remove(2);
                Assert.True(transaction.ChangesCount == 1);

                transaction.Add(2, "ipsum");
                Assert.True(transaction.ChangesCount == 0);
            }
            Assert.True(transactHashTable.Count == 3);
        }
    }
}
