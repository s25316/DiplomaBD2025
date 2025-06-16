'use client'
import Link from 'next/link';
import { useRouter } from 'next/navigation';

const BranchesList = ({ branches, companyId }: { branches: any[], companyId: string }) => {
  const router = useRouter();

  if (!branches.length) return <h2>No branches available</h2>;

  return (
    <ul>
      {branches.map((value) => (
        <li key={value.branch.branchId} className="border p-2 rounded my-2 max-w-md">
          {/* Flex container for horizontal layout */}
          <div className="flex justify-between items-center">
            <Link href={`/companies/${companyId}/${value.branch.branchId}`}>
              <b>Name: {value.branch.name}</b>
            </Link>
            <button
              onClick={() => router.push(`/companies/${companyId}/${value.branch.branchId}`)}
              className="bg-green-600 text-white px-3 py-1 rounded hover:bg-green-700"
            >
              To Create Offer
            </button>
          </div>
        </li>
      ))}
    </ul>
  );
};

export default BranchesList;
