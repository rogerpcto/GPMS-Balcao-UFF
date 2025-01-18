import Image from "next/image";
import {
  Carousel,
  CarouselContent,
  CarouselItem,
  CarouselNext,
  CarouselPrevious,
} from "@/components/ui/carousel";

interface ImageCarouselProps {
  images: string[];
}

export function ImageCarousel({ images }: ImageCarouselProps) {
  return (
    <Carousel className="w-full max-h-[480px] max-w-xs sm:max-w-sm md:max-w-md lg:max-w-lg xl:max-w-xl">
      <CarouselContent>
        {images.map((src, index) => (
          <CarouselItem key={index}>
            <div className="p-1">
              <Image
                src={src}
                alt={`Slide ${index + 1}`}
                width={800}
                height={600}
                className="w-full h-auto rounded-lg object-cover"
              />
            </div>
          </CarouselItem>
        ))}
      </CarouselContent>
      <CarouselPrevious />
      <CarouselNext />
    </Carousel>
  );
}
