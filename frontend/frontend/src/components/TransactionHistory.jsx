const TransactionHistory = ({ transactions }) => {
    // Add mock transactions for empty state
    const mockTransactions = [
        {
            _id: 'mock1',
            type: 'deposit',
            amount: 1000.00,
            balance: 1000.00,
            createdAt: new Date(Date.now() - 86400000).toISOString() // 1 day ago
        },
        {
            _id: 'mock2',
            type: 'withdrawal',
            amount: 250.00,
            balance: 750.00,
            createdAt: new Date(Date.now() - 43200000).toISOString() // 12 hours ago
        },
        {
            _id: 'mock3',
            type: 'deposit',
            amount: 500.00,
            balance: 1250.00,
            createdAt: new Date().toISOString() // now
        }
    ];

    // Use real transactions if available, otherwise use mock data
    const displayTransactions = transactions.length > 0 ? transactions : mockTransactions;

    return (
        <div className="bg-white p-6 rounded-lg shadow-md">
            <h3 className="text-xl font-semibold mb-4">Transaction History</h3>
            {transactions.length === 0 && (
                <p className="text-gray-500 mb-4">Showing sample transactions. Your actual transactions will appear here.</p>
            )}
            <div className="overflow-x-auto">
                <table className="min-w-full table-auto border-collapse">
                    <thead>
                        <tr className="bg-gray-100 border-b">
                            <th className="px-4 py-2 text-left">Date</th>
                            <th className="px-4 py-2 text-left">Type</th>
                            <th className="px-4 py-2 text-left">Amount</th>
                            <th className="px-4 py-2 text-left">Balance</th>
                        </tr>
                    </thead>
                    <tbody>
                        {displayTransactions.map((transaction) => {
                            // Convert date with error handling
                            let dateString = "N/A";
                            try {
                                if (transaction.createdAt) {
                                    dateString = new Date(transaction.createdAt).toLocaleString();
                                }
                            } catch (error) {
                                console.error("Error formatting date:", error);
                            }

                            return (
                                <tr key={transaction._id} className="border-b hover:bg-gray-50">
                                    <td className="px-4 py-2">{dateString}</td>
                                    <td
                                        className={`px-4 py-2 ₹{transaction.type === "deposit"
                                            ? "text-green-600 font-medium"
                                            : "text-red-600 font-medium"
                                            }`}
                                    >
                                        {transaction.type}
                                    </td>
                                    <td className="px-4 py-2">
                                        ₹{transaction.amount.toFixed(2)}
                                    </td>
                                    <td className="px-4 py-2">
                                        ₹{transaction.balance.toFixed(2)}
                                    </td>
                                </tr>
                            );
                        })}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default TransactionHistory;
