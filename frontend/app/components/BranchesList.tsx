'use client'
import { useSession } from 'next-auth/react';
import Link from 'next/link';
import { useParams, useRouter } from 'next/navigation';
import DeleteBranchButton from './buttons/DeleteBranchButton';
import { InnerSection } from './layout/PageContainers';

interface Branch {
  branchId: string;
  name: string;
  address: {
    cityName: string;
    streetName: string | null;
  }
}
interface BranchesListProps {
  branches: Branch[];
  companyId: string;
  onDelete: (id: string) => void;
}

const BranchesList = ({ 
  branches, companyId, onDelete,
}: BranchesListProps) => {
  const { data: session } = useSession();
  const router = useRouter();


  if (!branches.length) return <h2>No branches available</h2>;

  return (
    <ul>
      {branches.map((branch) => (
        <li key={branch.branchId} >
          <InnerSection className="my-2 max-w-md">
          {/* Flex container for horizontal layout */}
          {/* <div className="flex justify-between items-center"> */}
            <Link href={`/companies/${companyId}/${branch.branchId}`}>
              <b>Name: {branch.name}</b>
            </Link>
            <p><b>City: {branch.address.cityName}</b></p>
            <p><b>street: {branch.address.streetName}</b></p>
          <div className="mt-2 flex gap-2">

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

          </div>
          </InnerSection>
        </li>
        
      ))}
    </ul>
  );
};

export default BranchesList;
