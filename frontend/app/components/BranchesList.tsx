'use client'
import Link from 'next/link';
import { useRouter } from 'next/navigation';
import DeleteBranchButton from './buttons/DeleteBranchButton';
import { InnerSection } from './layout/PageContainers';

interface Branch {
  branchId: string;
  name: string;
  address: {
    cityName: string;
    streetName: string | null;
    houseNumber: string | null;
  }
}
interface BranchesListProps {
  branches: Branch[];
  companyId: string;
  onDelete: (id: string) => void;
  isOwner: boolean;
}

const BranchesList = ({ 
  branches, companyId, onDelete, isOwner
}: BranchesListProps) => {
  const router = useRouter();


  if (!branches.length) return <h2>No branches available</h2>;

  return (
    <ul>
      {branches.map((branch) => (
        <li key={branch.branchId} >
          <InnerSection className="my-2 max-w-md">
            <Link href={`/companies/${companyId}/${branch.branchId}`} className="text-blue-600 dark:text-blue-400">
              <b>Branch name: {branch.name}</b>
            </Link>
            <p><b>City:</b> {branch.address.cityName}</p>
            <p><b>street:</b> {branch.address.streetName} {branch.address.houseNumber}</p>
          {isOwner && <div className="mt-2 flex gap-2">

            <button
              onClick={() => router.push(`/companies/${companyId}/${branch.branchId}`)}
              className="bg-green-600 text-white px-3 py-1 rounded hover:bg-green-700"
            >
              Create/See Offers
            </button>
            
            <DeleteBranchButton
                branchId={branch.branchId}
                companyId={companyId} // Przekazujemy companyId do przekierowania
                onSuccess={() => onDelete(branch.branchId)} // Callback do CompanyDetails
                buttonText="Delete"
                className="bg-red-500 text-white px-3 py-1 rounded-md hover:bg-red-600 transition duration-200 shadow-sm"
              />

          </div>}
          </InnerSection>
        </li>
      ))}
    </ul>
  );
};

export default BranchesList;
